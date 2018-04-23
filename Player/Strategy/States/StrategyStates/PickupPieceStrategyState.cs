using Player.Strategy.Conditions.StrategyConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.StrategyStates
{
    public class PickupPieceStrategyState : StrategyState
    {
        public PickupPieceStrategyState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            transitionConditions.Add(new IsAbleToTestStrategyCondition(strategyInfo));
        }
    }
}