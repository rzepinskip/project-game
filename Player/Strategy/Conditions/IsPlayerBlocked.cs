using System;
using Common;
using Common.ActionAvailability.AvailabilityChain;
using Common.ActionAvailability.Helpers;
using Common.BoardObjects;
using Messaging.Requests;
using Messaging.Responses;
using Player.Strategy.States;

namespace Player.Strategy.Conditions
{
    class IsPlayerBlocked : Condition
    {

        private readonly Random _directionGenerator;

        public IsPlayerBlocked(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            _directionGenerator = new Random();
        }

        public override bool CheckCondition()
        {
            return !StrategyInfo.FromLocation.Equals(StrategyInfo.ToLocation);
        }

        public override State GetNextState(State fromState)
        {
            switch (fromState)
            {
            case InGoalAreaMovingToTaskState inGoalAreaMovingToTaskState:
                return new InGoalAreaMovingToTaskState(StrategyInfo);
            case MoveToPieceState moveToPieceState:
                return new MoveToPieceState(StrategyInfo);
            case MoveToUndiscoveredGoalState moveToUndiscoveredGoalState:
                return new MoveToUndiscoveredGoalState(StrategyInfo);
            default:
                throw new StrategyException("Unknown state", fromState, StrategyInfo);
            }
        }

        public override Request GetNextMessage(State fromState)
        {
            var direction = default(Direction);
            var currentLocation = StrategyInfo.FromLocation;
            
            switch (fromState)
            {
                case InGoalAreaMovingToTaskState inGoalAreaMovingToTaskState:
                {
                    direction = _directionGenerator.Next() % 2 == 0 ? Direction.Left : Direction.Right;
                    break;
                }

                case MoveToPieceState moveToPieceState:
                {
                    direction = Randomize4WayDirection(StrategyInfo, true);
                    break;
                }
                case MoveToUndiscoveredGoalState moveToUndiscoveredGoalState:
                {
                    direction = Randomize4WayDirection(StrategyInfo, false);
                    break;
                }
                default:
                    throw new StrategyException("Unknown state:", fromState, StrategyInfo);
            }

            StrategyInfo.ToLocation = StrategyInfo.FromLocation.GetNewLocation(direction);
            return new MoveRequest(StrategyInfo.PlayerId, direction);
        }

        private Direction Randomize4WayDirection(StrategyInfo strategyInfo, bool onlyTaskArea)
        {
            var currentLocation = strategyInfo.FromLocation;
            var desiredLocation = strategyInfo.ToLocation;
            var directionValue = (_directionGenerator.Next() % 4);
            var direction = (Direction)directionValue;
            var newLocation = currentLocation.GetNewLocation(direction);
            while (desiredLocation.Equals(currentLocation.GetNewLocation(direction)) || !new MoveAvailabilityChain(currentLocation, direction, StrategyInfo.Team, StrategyInfo.Board).ActionAvailable() || (onlyTaskArea && !StrategyInfo.Board.IsLocationInTaskArea(newLocation)))
            {
                directionValue = (directionValue + 1) % 4;
                direction = (Direction)directionValue;
                newLocation = currentLocation.GetNewLocation(direction);
            }

            return direction;
        }
    }
}
