using Player.Strategy.Conditions.StrategyConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.StrategyStates
{
    public class InitStrategyState : StrategyState
    {
        public InitStrategyState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            transitionConditions.Add(new IsInGoalWithoutPieceStrategyCondition(strategyInfo));
            transitionConditions.Add(new IsInTaskAreaStrategyCondition(strategyInfo));
        }
    }
}