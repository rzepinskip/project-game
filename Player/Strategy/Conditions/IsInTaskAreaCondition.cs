using Messaging.Requests;
using Player.Strategy.States;

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

        public override Request GetNextMessage(State fromState)
        {
            return new DiscoverRequest(StrategyInfo.PlayerId);
        }
    }
}