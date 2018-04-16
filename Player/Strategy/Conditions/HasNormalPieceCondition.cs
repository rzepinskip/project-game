using Common;
using Messaging.Requests;
using Player.Strategy.States;

namespace Player.Strategy.Conditions
{
    public class HasNormalPieceCondition : Condition
    {
        public HasNormalPieceCondition(StrategyInfo strategyInfo) : base(strategyInfo)
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

        public override State GetNextState(State fromState)
        {
            return new MoveToUndiscoveredGoalState(StrategyInfo);
        }

        public override Request GetNextMessage(State fromState)
        {
            var undiscoveredGoalLocation = StrategyInfo.UndiscoveredGoalFields[0];
            var direction = undiscoveredGoalLocation.GetDirectionFrom(StrategyInfo.FromLocation);
            StrategyInfo.ToLocation = StrategyInfo.FromLocation.GetNewLocation(direction);
            return new MoveRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId, direction);
        }
    }
}