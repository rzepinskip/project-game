﻿using System;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.StrategyConditions
{
    public class IsNoPieceAvailableStrategyCondition : StrategyCondition
    {
        private readonly Random _directionGenerator;

        public IsNoPieceAvailableStrategyCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            _directionGenerator = new Random();
        }

        public override bool CheckCondition()
        {
            var taskField =
                StrategyInfo.Board[StrategyInfo.FromLocation] as TaskField;
            var result = false;
            if (taskField != null)
            {
                var distanceToNearestPiece = taskField.DistanceToPiece;
                if (distanceToNearestPiece == -1)
                    result = true;
            }

            return result;
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new MoveToPieceStrategyState(StrategyInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            var direction = _directionGenerator.Next() % 2 == 0
                ? Direction.Left
                : Direction.Right;
            StrategyInfo.ToLocation = StrategyInfo.FromLocation.GetNewLocation(direction);
            return new MoveRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId, direction);
        }

        public override bool ReturnsMessage(BaseState fromStrategyState)
        {
            return true;
        }
    }
}