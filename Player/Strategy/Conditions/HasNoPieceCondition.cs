using Player.Strategy.States;
using Shared.GameMessages;

namespace Player.Strategy.Conditions
{
    public class HasNoPieceCondition : Condition
    {
        public HasNoPieceCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            var playerInfo = StrategyInfo.Board.Players[StrategyInfo.PlayerId];
            return playerInfo.Piece == null;
        }

        public override State GetNextState(State fromState)
        {
            return new DiscoverState(StrategyInfo);
        }

        public override GameMessage GetNextMessage(State fromState)
        {
            return new Discover
            {
                PlayerId = StrategyInfo.PlayerId
            };
        }
    }
}