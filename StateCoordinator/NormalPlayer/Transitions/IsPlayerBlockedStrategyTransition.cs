using System;
using System.Collections.Generic;
using ClientsCommon.ActionAvailability.AvailabilityChain;
using Common;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.States;
using PlayerStateCoordinator.TeamLeader;

namespace PlayerStateCoordinator.NormalPlayer.Transitions
{
    public class IsPlayerBlockedStrategyTransition : GameStrategyTransition
    {
        private readonly Random _directionGenerator;
        private readonly GamePlayStrategyState _fromState;
        private Direction? _chosenDirection;
        private bool _isAnyMoveAvailable;
        public IsPlayerBlockedStrategyTransition(GamePlayStrategyInfo gamePlayStrategyInfo, GamePlayStrategyState fromState)
            : base(
                gamePlayStrategyInfo)
        {
            _directionGenerator = new Random();
            _fromState = fromState;
            _chosenDirection = null;
        }

        public override State NextState
        {
            get
            {
                if (!_chosenDirection.HasValue)
                {
                    _chosenDirection = Randomize4WayDirection(GamePlayStrategyInfo);
                }

                if (_isAnyMoveAvailable)
                {
                    //Console.WriteLine($"PlayerBlocked returning to {_fromState}");
                    if (_fromState.TransitionType == StateTransitionType.Immediate)
                        throw new StrategyException(_fromState,
                            "IsPlayerBlocked transition cannot proceed to Immediate state! - an error in designing strategy");
                    if (_fromState is NormalPlayerStrategyState)
                    {
                        Console.WriteLine("Recognized normal state");
                        return Activator.CreateInstance(_fromState.GetType(),
                            GamePlayStrategyInfo) as NormalPlayerStrategyState;
                    }

                    if (_fromState is LeaderStrategyState)
                    {
                        Console.WriteLine("Recognized leader state");
                        return Activator.CreateInstance(_fromState.GetType(),
                            GamePlayStrategyInfo) as LeaderStrategyState;
                    }
                }

                return new DiscoverStrategyState(GamePlayStrategyInfo);
            }
        }

        public override IEnumerable<IMessage> Message
        {
            get
            {
                if (!_chosenDirection.HasValue)
                {
                    _chosenDirection = Randomize4WayDirection(GamePlayStrategyInfo);
                }

                var message = default(IMessage);
                if (!_isAnyMoveAvailable)
                {
                    message = new DiscoverRequest(GamePlayStrategyInfo.PlayerGuid, GamePlayStrategyInfo.GameId);
                }
                else
                {
                    GamePlayStrategyInfo.TargetLocation = GamePlayStrategyInfo.CurrentLocation.GetNewLocation(_chosenDirection.Value);
                    message = new MoveRequest(GamePlayStrategyInfo.PlayerGuid, GamePlayStrategyInfo.GameId, _chosenDirection.Value);
                }

                return new List<IMessage>
                {
                    message
                };
            }
        }

        private Direction Randomize4WayDirection(GamePlayStrategyInfo strategyInfo)
        {
            var onlyTaskArea = false;
            switch (_fromState)
            {
                case MoveToPieceStrategyState moveToPieceState:
                {
                    onlyTaskArea = true;
                    break;
                }
                case InGoalAreaMovingToTaskStrategyState inGoalAreaMovingToTaskState:
                case MoveToUndiscoveredGoalStrategyState moveToUndiscoveredGoalState:
                case InitialMoveAfterPlaceStrategyState initialMoveAfterPlaceStrategyState:
                    break;
                default:
                    Console.WriteLine("Unexpeted state in PlayerBlocked transition");
                    break;
            }

            _isAnyMoveAvailable = true;
            var currentLocation = GamePlayStrategyInfo.CurrentLocation;
            var desiredLocation = GamePlayStrategyInfo.TargetLocation;
            var numberOfDirections = Enum.GetNames(typeof(Direction)).Length;
            var directionValue = _directionGenerator.Next(numberOfDirections);
            var direction = (Direction) directionValue;
            var newLocation = currentLocation.GetNewLocation(direction);
            var checkDirectionsCounter = 0;
            while (desiredLocation.Equals(currentLocation.GetNewLocation(direction)) ||
                   !new MoveAvailabilityChain(currentLocation, direction, GamePlayStrategyInfo.Team, GamePlayStrategyInfo.Board)
                       .ActionAvailable() || onlyTaskArea && !GamePlayStrategyInfo.Board.IsLocationInTaskArea(newLocation))
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