using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.StrategyConditions
{
    public class IsOnFieldWithPieceStrategyCondition : StrategyCondition
    {
        public IsOnFieldWithPieceStrategyCondition(StrategyInfo strategyInfo) : base(strategyInfo)
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

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new PickupPieceStrategyState(StrategyInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            return new PickUpPieceRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId);
        }
    }
}