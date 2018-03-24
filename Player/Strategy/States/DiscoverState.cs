using Player.Strategy.Conditions;

namespace Player.Strategy.States
{
    public class DiscoverState : State
    {
        public DiscoverState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            conditions.Add(new MoveCondition(strategyInfo));
        }
    }
}