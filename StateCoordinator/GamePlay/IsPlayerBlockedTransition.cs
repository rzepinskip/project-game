using System;
using System.Collections.Generic;
using ClientsCommon.ActionAvailability.AvailabilityChain;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;

namespace PlayerStateCoordinator.GamePlay
{
    public abstract class IsPlayerBlockedTransition : GameStrategyTransition
    {
        private readonly Random _directionGenerator;
        protected readonly GamePlayStrategyState FromState;
        private Direction? _chosenDirection;
        private bool _isAnyMoveAvailable;

        protected IsPlayerBlockedTransition(GamePlayStrategyInfo gamePlayStrategyInfo,
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
                if (!_chosenDirection.HasValue) _chosenDirection = Randomize4WayDirection();

                if (!_isAnyMoveAvailable) return NextStateForFullyBlockedCase;

                //Console.WriteLine($"PlayerBlocked returning to {_fromState}");
                if (FromState.TransitionType == StateTransitionType.Immediate)
                    throw new StrategyException(FromState,
                        "IsPlayerBlocked transition cannot proceed to Immediate state! - an error in designing strategy");

                //Console.WriteLine("Recognized normal state");

                return Activator.CreateInstance(FromState.GetType(), GamePlayStrategyInfo) as GamePlayStrategyState;
            }
        }

        public override IEnumerable<IMessage> Message
        {
            get
            {
                if (!_chosenDirection.HasValue) _chosenDirection = Randomize4WayDirection();

                IMessage message;
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

        protected abstract GamePlayStrategyState NextStateForFullyBlockedCase { get; }

        protected abstract void CheckIfFromStateIsPredicted(GamePlayStrategyState FromState);

        public override bool IsPossible()
        {
            return !GamePlayStrategyInfo.CurrentLocation.Equals(GamePlayStrategyInfo.TargetLocation);
        }

        private Direction? Randomize4WayDirection()
        {
            var currentLocation = GamePlayStrategyInfo.CurrentLocation;
            var numberOfDirections = Enum.GetNames(typeof(Direction)).Length;
            var directionValue = _directionGenerator.Next(numberOfDirections);
            var direction = (Direction) directionValue;
            var newLocation = currentLocation.GetNewLocation(direction);
            var checkDirectionsCounter = 0;
            while (!IsRandomlyChosenDirectionAppriopriate(direction, currentLocation, newLocation))
            {
                directionValue = (directionValue + 1) % numberOfDirections;
                direction = (Direction) directionValue;
                newLocation = currentLocation.GetNewLocation(direction);
                checkDirectionsCounter++;

                if (checkDirectionsCounter >= numberOfDirections)
                {
                    _isAnyMoveAvailable = false;
                    break;
                }
            }

            _isAnyMoveAvailable = true;
            return direction;
        }

        private bool IsRandomlyChosenDirectionAppriopriate(Direction direction, Location currentLocation,
            Location newLocation)
        {
            var desiredLocation = GamePlayStrategyInfo.TargetLocation;
            var isAvailableMove = new MoveAvailabilityChain(currentLocation, direction, GamePlayStrategyInfo.Team,
                    GamePlayStrategyInfo.Board)
                .ActionAvailable();
            var isLocationInTaskArea = GamePlayStrategyInfo.Board.IsLocationInTaskArea(newLocation);
            var isLocationInAllowedAreaForState = !FromState.RestrictedToTaskArea || isLocationInTaskArea;
            var isBlockedDirection = desiredLocation.Equals(currentLocation.GetNewLocation(direction));
            return isAvailableMove && !isBlockedDirection && isLocationInAllowedAreaForState;
        }
    }
}