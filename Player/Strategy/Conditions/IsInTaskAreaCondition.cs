using Player.Strategy.States;
using Shared.GameMessages;

namespace Player.Strategy.Conditions
{
    public class IsInTaskAreaCondition : Condition
    {
        public IsInTaskAreaCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            return StrategyInfo.Board.IsLocationInTaskArea(StrategyInfo.FromLocation);
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