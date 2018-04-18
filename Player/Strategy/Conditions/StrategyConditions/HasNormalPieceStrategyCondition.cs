using Common;
using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.StrategyConditions
{
    public class HasNormalPieceStrategyCondition : StrategyCondition
    {
        public HasNormalPieceStrategyCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            var currentLocation = StrategyInfo.FromLocation;
            var goalLocation = StrategyInfo.UndiscoveredGoalFields[0];
            var playerInfo = StrategyInfo.Board.Players[StrategyInfo.PlayerId];
            var piece = playerInfo.Piece;
            var result = false;
            if (piece != null && !currentLocation.Equals(goalLocation))
                if (piece.Type == PieceType.Normal)
                    result = true;
            return result;
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new MoveToUndiscoveredGoalStrategyState(StrategyInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            var undiscoveredGoalLocation = StrategyInfo.UndiscoveredGoalFields[0];
            var direction = undiscoveredGoalLocation.GetDirectionFrom(StrategyInfo.FromLocation);
            StrategyInfo.ToLocation = StrategyInfo.FromLocation.GetNewLocation(direction);
            return new MoveRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId, direction);
        }
    }
}