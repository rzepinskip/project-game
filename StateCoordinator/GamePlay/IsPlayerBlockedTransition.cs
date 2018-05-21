using System;
using System.Collections.Generic;
using ClientsCommon.ActionAvailability.AvailabilityChain;
using Common;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GamePlay.NormalPlayer.States;

namespace PlayerStateCoordinator.GamePlay
{
    public abstract class IsPlayerBlockedTransition : GameStrategyTransition
    {
        private readonly Random _directionGenerator;
        protected readonly GamePlayStrategyState FromState;
        private Direction? _chosenDirection;
        private bool _isAnyMoveAvailable;

        public IsPlayerBlockedTransition(GamePlayStrategyInfo gamePlayStrategyInfo,
            GamePlayStrategyState fromState)
            : base(
                gamePlayStrategyInfo)
        {
            _directionGenerator = new Random();
            FromState = fromState;
            _chosenDirection = null;
        }

        public override State NextState
        {
            get
            {
                if (!_chosenDirection.HasValue) _chosenDirection = Randomize4WayDirection(GamePlayStrategyInfo);

                if (_isAnyMoveAvailable)
                {
                    //Console.WriteLine($"PlayerBlocked returning to {_fromState}");
                    if (FromState.TransitionType == StateTransitionType.Immediate)
                        throw new StrategyException(FromState,
                            "IsPlayerBlocked transition cannot proceed to Immediate state! - an error in designing strategy");
                    

                    Console.WriteLine("Recognized normal state");

                    return GetFromState();

                }
                //return new DiscoverStrategyState(GamePlayStrategyInfo);
                return GetRecoveryFromBlockedState();
            }
        }

        protected abstract GamePlayStrategyState GetRecoveryFromBlockedState();

        protected abstract GamePlayStrategyState GetFromState();
        protected abstract bool IsFromStateOnlyInTaskArea(GamePlayStrategyState fromState);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                if (!_chosenDirection.HasValue) _chosenDirection = Randomize4WayDirection(GamePlayStrategyInfo);

                var message = default(IMessage);
                if (!_isAnyMoveAvailable)
                {
                    message = new DiscoverRequest(GamePlayStrategyInfo.PlayerGuid, GamePlayStrategyInfo.GameId);
                }
                else
                {
                    GamePlayStrategyInfo.TargetLocation =
                        GamePlayStrategyInfo.CurrentLocation.GetNewLocation(_chosenDirection.Value);
                    message = new MoveRequest(GamePlayStrategyInfo.PlayerGuid, GamePlayStrategyInfo.GameId,
                        _chosenDirection.Value);
                }

                return new List<IMessage>
                {
                    message
                };
            }
        }

        private Direction Randomize4WayDirection(GamePlayStrategyInfo strategyInfo)
        {
            var onlyTaskArea = IsFromStateOnlyInTaskArea(FromState);

            _isAnyMoveAvailable = true;
            var currentLocation = GamePlayStrategyInfo.CurrentLocation;
            var desiredLocation = GamePlayStrategyInfo.TargetLocation;
            var numberOfDirections = Enum.GetNames(typeof(Direction)).Length;
            var directionValue = _directionGenerator.Next(numberOfDirections);
            var direction = (Direction) directionValue;
            var newLocation = currentLocation.GetNewLocation(direction);
            var checkDirectionsCounter = 0;
            while (desiredLocation.Equals(currentLocation.GetNewLocation(direction)) ||
                   !new MoveAvailabilityChain(currentLocation, direction, GamePlayStrategyInfo.Team,
                           GamePlayStrategyInfo.Board)
                       .ActionAvailable() ||
                   onlyTaskArea && !GamePlayStrategyInfo.Board.IsLocationInTaskArea(newLocation))
            {
                directionValue = (directionValue + 1) % 4;
                direction = (Direction) directionValue;
                newLocation = currentLocation.GetNewLocation(direction);
                checkDirectionsCounter++;

                if (checkDirectionsCounter >= 4)
                {
                    _isAnyMoveAvailable = false;
                    break;
                }
            }

            return direction;
        }

        public override bool IsPossible()
        {
            return !GamePlayStrategyInfo.CurrentLocation.Equals(GamePlayStrategyInfo.TargetLocation);
        }
    }
}