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
        private readonly State _fromState;
        public IsPlayerBlockedStrategyTransition(GameStrategyInfo gameStrategyInfo, State fromState) : base(gameStrategyInfo)
        {
            _directionGenerator = new Random();
            _fromState = fromState;
        }

        public override State NextState
        {
            get
            {
                var onlyTaskArea = false;
                Randomize4WayDirection(GameStrategyInfo, onlyTaskArea, out var isAnyMoveAvailable);
                if (isAnyMoveAvailable)
                    switch (_fromState)
                    {
                        case InGoalAreaMovingToTaskStrategyState inGoalAreaMovingToTaskState:
                            return new InGoalAreaMovingToTaskStrategyState(GameStrategyInfo);
                        case MoveToPieceStrategyState moveToPieceState:
                            return new MoveToPieceStrategyState(GameStrategyInfo);
                        case MoveToUndiscoveredGoalStrategyState moveToUndiscoveredGoalState:
                            return new MoveToUndiscoveredGoalStrategyState(GameStrategyInfo);
                        default:
                            throw new StrategyException(_fromState, "Unknown state");
                    }
                return new DiscoverStrategyState(GameStrategyInfo);
            }
        }

        public override IEnumerable<IMessage> Message
        {
            get
            {
                var direction = default(Direction);
                var onlyTaskArea = false;
                var message = default(IMessage);
                switch (_fromState)
                {
                    case MoveToPieceStrategyState moveToPieceState:
                    {
                        onlyTaskArea = true;
                        break;
                    }
                    case InGoalAreaMovingToTaskStrategyState inGoalAreaMovingToTaskState:
                    case MoveToUndiscoveredGoalStrategyState moveToUndiscoveredGoalState:
                        break;

                    default:
                        throw new StrategyException(_fromState, "Unknown state");
                }

                direction = Randomize4WayDirection(GameStrategyInfo, onlyTaskArea, out var isAnyMoveAvailable);
                if (!isAnyMoveAvailable)
                    message = new DiscoverRequest(GameStrategyInfo.PlayerGuid, GameStrategyInfo.GameId);
                else
                {
                    GameStrategyInfo.TargetLocation = GameStrategyInfo.CurrentLocation.GetNewLocation(direction);
                    message = new MoveRequest(GameStrategyInfo.PlayerGuid, GameStrategyInfo.GameId, direction);
                }

                return new List<IMessage>()
                {
                    message
                };
            }
        }

        private Direction Randomize4WayDirection(GameStrategyInfo strategyInfo, bool onlyTaskArea,
            out bool isAnyMoveAvailable)
        {
            isAnyMoveAvailable = true;
            var currentLocation = GameStrategyInfo.CurrentLocation;
            var desiredLocation = GameStrategyInfo.TargetLocation;
            var directionValue = 0;
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
                    isAnyMoveAvailable = false;
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