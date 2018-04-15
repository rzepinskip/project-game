using System;
using Common;
using Common.BoardObjects;
using Messaging.Requests;
using Player.Strategy.States;

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

        public override State GetNextState(State fromState)
        {
            return new MoveToPieceState(StrategyInfo);
        }

        public override Request GetNextMessage(State fromState)
        {
            var direction = _directionGenerator.Next() % 2 == 0
                ? Direction.Left
                : Direction.Right;
            StrategyInfo.ToLocation = StrategyInfo.FromLocation.GetNewLocation(direction);
            return new MoveRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId, direction);
        }
    }
}