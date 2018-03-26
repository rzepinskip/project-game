using Player.Strategy.Conditions;

namespace Player.Strategy.States
{
    public class InGoalAreaMovingToTaskState : State
    {
        public InGoalAreaMovingToTaskState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            conditions.Add(new IsPlayerBlocked(strategyInfo));
            conditions.Add(new IsInGoalWithoutPieceCondition(strategyInfo));
            conditions.Add(new IsInTaskAreaCondition(strategyInfo));
        }
    }
}