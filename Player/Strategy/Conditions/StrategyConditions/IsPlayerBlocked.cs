﻿using System;
using Common;
using Common.ActionAvailability.AvailabilityChain;
using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.StrategyConditions
{
    internal class IsPlayerBlocked : StrategyCondition
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

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            switch (fromStrategyState)
            {
                case InGoalAreaMovingToTaskStrategyState inGoalAreaMovingToTaskState:
                    return new InGoalAreaMovingToTaskStrategyState(StrategyInfo);
                case MoveToPieceStrategyState moveToPieceState:
                    return new MoveToPieceStrategyState(StrategyInfo);
                case MoveToUndiscoveredGoalStrategyState moveToUndiscoveredGoalState:
                    return new MoveToUndiscoveredGoalStrategyState(StrategyInfo);
                default:
                    throw new StrategyException("Unknown state", fromStrategyState, StrategyInfo);
            }
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            var direction = default(Direction);
            var onlyTaskArea = false;
            switch (fromStrategyState)
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
                    throw new StrategyException("Unknown state", fromStrategyState, StrategyInfo);
            }

            direction = Randomize4WayDirection(StrategyInfo, onlyTaskArea);
            StrategyInfo.ToLocation = StrategyInfo.FromLocation.GetNewLocation(direction);
            return new MoveRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId, direction);
        }

        public override bool ReturnsMessage(BaseState fromStrategyState)
        {
            return true;
        }

        private Direction Randomize4WayDirection(StrategyInfo strategyInfo, bool onlyTaskArea)
        {
            var currentLocation = strategyInfo.FromLocation;
            var desiredLocation = strategyInfo.ToLocation;
            var directionValue = _directionGenerator.Next() % 4;
            var direction = (Direction) directionValue;
            var newLocation = currentLocation.GetNewLocation(direction);
            while (desiredLocation.Equals(currentLocation.GetNewLocation(direction)) ||
                   !new MoveAvailabilityChain(currentLocation, direction, StrategyInfo.Team, StrategyInfo.Board)
                       .ActionAvailable() || onlyTaskArea && !StrategyInfo.Board.IsLocationInTaskArea(newLocation))
            {
                directionValue = (directionValue + 1) % 4;
                direction = (Direction) directionValue;
                newLocation = currentLocation.GetNewLocation(direction);
            }

            return direction;
        }
    }
}