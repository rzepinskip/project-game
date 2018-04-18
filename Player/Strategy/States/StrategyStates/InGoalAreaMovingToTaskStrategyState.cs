using Player.Strategy.Conditions.StrategyConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.StrategyStates
{
    public class InGoalAreaMovingToTaskStrategyState : StrategyState
    {
        public InGoalAreaMovingToTaskStrategyState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            conditions.Add(new IsPlayerBlocked(strategyInfo));
            conditions.Add(new IsInGoalWithoutPieceStrategyCondition(strategyInfo));
            conditions.Add(new IsInTaskAreaStrategyCondition(strategyInfo));
        }
    }
}