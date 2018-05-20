using System;
using System.Collections.Generic;
using ClientsCommon.ActionAvailability.AvailabilityChain;
using Common;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.States.GameStrategyStates;

namespace PlayerStateCoordinator.Transitions.GameStrategyTransitions
{
    public class IsPlayerBlockedStrategyTransition : GameStrategyTransition
    {
        private readonly Random _directionGenerator;
        private readonly GameStrategyState _fromState;
        private Direction _chosenDirection;
        private bool _isAnyMoveAvailable;
        private bool _isDirectionChosen;
        public IsPlayerBlockedStrategyTransition(GameStrategyInfo gameStrategyInfo, GameStrategyState fromState) : base(
            gameStrategyInfo)
        {
            _directionGenerator = new Random();
            _fromState = fromState;
            _isDirectionChosen = false;
        }

        public override State NextState
        {
            get
            {
                if (!_isDirectionChosen)
                {
                    _chosenDirection = Randomize4WayDirection(GameStrategyInfo);
                    _isDirectionChosen = true;
                }

                if (_isAnyMoveAvailable)
                {
                    Console.WriteLine($"PlayerBlocked returning to {_fromState}");
                    if (_fromState.TransitionType == StateTransitionType.Immediate)
                        throw new StrategyException(_fromState,
                            "IsPlayerBlocked transition cannot proceed to Immediate state! - an error in designing strategy");
                    return Activator.CreateInstance(_fromState.GetType(), GameStrategyInfo) as GameStrategyState;
                }

                return new DiscoverStrategyState(GameStrategyInfo);
            }
        }

        public override IEnumerable<IMessage> Message
        {
            get
            {
                if (!_isDirectionChosen)
                {
                    _chosenDirection = Randomize4WayDirection(GameStrategyInfo);
                    _isDirectionChosen = true;
                }

                var message = default(IMessage);
                if (!_isAnyMoveAvailable)
                {
                    message = new DiscoverRequest(GameStrategyInfo.PlayerGuid, GameStrategyInfo.GameId);
                }
                else
                {
                    GameStrategyInfo.TargetLocation = GameStrategyInfo.CurrentLocation.GetNewLocation(_chosenDirection);
                    message = new MoveRequest(GameStrategyInfo.PlayerGuid, GameStrategyInfo.GameId, _chosenDirection);
                }

                return new List<IMessage>
                {
                    message
                };
            }
        }

        private Direction Randomize4WayDirection(GameStrategyInfo strategyInfo)
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
            var currentLocation = GameStrategyInfo.CurrentLocation;
            var desiredLocation = GameStrategyInfo.TargetLocation;
            var numberOfDirections = Enum.GetNames(typeof(Direction)).Length;
            var directionValue = _directionGenerator.Next(numberOfDirections);
            var direction = (Direction) directionValue;
            var newLocation = currentLocation.GetNewLocation(direction);
            var checkDirectionsCounter = 0;
            while (desiredLocation.Equals(currentLocation.GetNewLocation(direction)) ||
                   !new MoveAvailabilityChain(currentLocation, direction, GameStrategyInfo.Team, GameStrategyInfo.Board)
                       .ActionAvailable() || onlyTaskArea && !GameStrategyInfo.Board.IsLocationInTaskArea(newLocation))
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
            return !GameStrategyInfo.CurrentLocation.Equals(GameStrategyInfo.TargetLocation);
        }
    }
}