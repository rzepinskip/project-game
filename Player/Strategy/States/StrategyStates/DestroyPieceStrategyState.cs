using Player.Strategy.Conditions.StrategyConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.StrategyStates
{
    public class DestroyPieceStrategyState : StrategyState
    {
        public DestroyPieceStrategyState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            transitionConditions.Add(new HasNoPieceStrategyCondition(strategyInfo));
        }
    }
}