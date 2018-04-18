using Player.Strategy.Conditions.StrategyConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.StrategyStates
{
    public class MoveToUndiscoveredGoalStrategyState : StrategyState
    {
        public MoveToUndiscoveredGoalStrategyState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            transitionConditions.Add(new IsPlayerBlocked(strategyInfo));
            transitionConditions.Add(new IsOnUndiscoveredGoalFieldStrategyCondition(strategyInfo));
            transitionConditions.Add(new HasNormalPieceStrategyCondition(strategyInfo));
        }
    }
}