﻿using Player.Strategy.States;
using Shared.BoardObjects;
using Shared.GameMessages;

namespace Player.Strategy.Conditions
{
    public class IsOnFieldWithPieceCondition : Condition
    {
        public IsOnFieldWithPieceCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            var taskField =
                StrategyInfo.Board.Content[StrategyInfo.FromLocation.X, StrategyInfo.FromLocation.Y] as TaskField;
            var result = false;
            if (taskField != null)
            {
                var distanceToNearestPiece = taskField.DistanceToPiece;
                if (distanceToNearestPiece == 0)
                    result = true;
            }

            return result;
        }

        public override State GetNextState(State fromState)
        {
            return new PickupPieceState(StrategyInfo);
        }

        public override GameMessage GetNextMessage(State fromState)
        {
            return new PickUpPiece
            {
                PlayerId = StrategyInfo.PlayerId
            };
        }
    }
}