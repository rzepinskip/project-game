using Player.Strategy.Conditions;

namespace Player.Strategy.States
{
    public class TestPieceState : State
    {
        public TestPieceState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            conditions.Add(new HasNoPieceCondition(strategyInfo));
            conditions.Add(new HasNormalPieceCondition(strategyInfo));
        }
    }
}