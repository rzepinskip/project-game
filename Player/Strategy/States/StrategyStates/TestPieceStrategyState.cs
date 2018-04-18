using Player.Strategy.Conditions.StrategyConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.StrategyStates
{
    public class TestPieceStrategyState : StrategyState
    {
        public TestPieceStrategyState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            conditions.Add(new HasNoPieceStrategyCondition(strategyInfo));
            conditions.Add(new HasNormalPieceStrategyCondition(strategyInfo));
        }
    }
}