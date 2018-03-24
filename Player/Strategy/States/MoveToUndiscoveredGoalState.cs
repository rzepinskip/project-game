using Player.Strategy.Conditions;

namespace Player.Strategy.States
{
    public class MoveToUndiscoveredGoalState : State
    {
        public MoveToUndiscoveredGoalState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            conditions.Add(new IsOnUndiscoveredGoalFieldCondition(strategyInfo));
            conditions.Add(new HasNormalPieceCondition(strategyInfo));
        }
    }
}