using Player.Strategy.Conditions.StrategyConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.StrategyStates
{
    public class MoveToUndiscoveredGoalStrategyState : StrategyState
    {
        public MoveToUndiscoveredGoalStrategyState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            conditions.Add(new IsPlayerBlocked(strategyInfo));
            conditions.Add(new IsOnUndiscoveredGoalFieldStrategyCondition(strategyInfo));
            conditions.Add(new HasNormalPieceStrategyCondition(strategyInfo));
        }
    }
}