using Common.BoardObjects;
using Messaging.Requests;
using Player.Strategy.States;

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
                StrategyInfo.Board[StrategyInfo.FromLocation] as TaskField;
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

        public override Request GetNextMessage(State fromState)
        {
            return new PickUpPieceRequest(StrategyInfo.PlayerGuid);
        }
    }
}