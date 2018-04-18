using Player.Strategy.Conditions.StrategyConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.StrategyStates
{
    public class DiscoverStrategyState : StrategyState
    {
        public DiscoverStrategyState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            conditions.Add(new MoveStrategyCondition(strategyInfo));
        }
    }
}