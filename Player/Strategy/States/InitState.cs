using Player.Strategy.Conditions;

namespace Player.Strategy.States
{
    public class InitState : State
    {
        public InitState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            conditions.Add(new IsInGoalWithoutPieceCondition(strategyInfo));
            conditions.Add(new IsInTaskAreaCondition(strategyInfo));
        }
    }
}