using System;
using Player.Strategy.States;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;

namespace Player.Strategy.Conditions
{
    public class IsNoPieceAvailableCondition : Condition
    {
        private readonly Random _directionGenerator;

        public IsNoPieceAvailableCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            _directionGenerator = new Random();
        }

        public override bool CheckCondition()
        {
            var taskField =
                StrategyInfo.Board.Content[StrategyInfo.FromLocation.X, StrategyInfo.FromLocation.Y] as TaskField;
            var result = false;
            if (taskField != null)
            {
                var distanceToNearestPiece = taskField.DistanceToPiece;
                if (distanceToNearestPiece == -1)
                    result = true;
            }

            return result;
        }

        public override State GetNextState(State fromState)
        {
            return new MoveToPieceState(StrategyInfo);
        }

        public override GameMessage GetNextMessage(State fromState)
        {
            var direction = _directionGenerator.Next() % 2 == 0
                ? CommonResources.MoveType.Left
                : CommonResources.MoveType.Right;
            return new Move
            {
                Direction = direction,
                PlayerId = StrategyInfo.PlayerId
            };
        }
    }
}