using Player.Strategy.Conditions;

namespace Player.Strategy.States
{
    public class MoveToPieceState : State
    {
        public MoveToPieceState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            conditions.Add(new IsOnFieldWithNoPieceCondition(strategyInfo));
            conditions.Add(new IsNoPieceAvailableCondition(strategyInfo));
            conditions.Add(new IsOnFieldWithPieceCondition(strategyInfo));
        }
    }
}