using Player.Strategy.States;
using Shared;
using Shared.GameMessages;

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
                if (piece.Type == CommonResources.PieceType.Normal)
                    result = true;
            return result;
        }

        public override State GetNextState(State fromState)
        {
            return new MoveToUndiscoveredGoalState(StrategyInfo);
        }

        public override GameMessage GetNextMessage(State fromState)
        {
            var undiscoveredGoalLocation = StrategyInfo.UndiscoveredGoalFields[0];

            return new Move
            {
                PlayerId = StrategyInfo.PlayerId,
                Direction = undiscoveredGoalLocation.GetDirectionFrom(StrategyInfo.FromLocation)
            };
        }
    }
}