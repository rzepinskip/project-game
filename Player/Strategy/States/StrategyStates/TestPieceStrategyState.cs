using Player.Strategy.Conditions.StrategyConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.StrategyStates
{
    public class TestPieceStrategyState : StrategyState
    {
        public TestPieceStrategyState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            transitionConditions.Add(new HasNoPieceStrategyCondition(strategyInfo));
            transitionConditions.Add(new HasNormalPieceStrategyCondition(strategyInfo));
            transitionConditions.Add(new HasShamCondition(strategyInfo));
        }
    }
}