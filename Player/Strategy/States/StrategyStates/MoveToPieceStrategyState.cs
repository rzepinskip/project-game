using Player.Strategy.Conditions.StrategyConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.StrategyStates
{
    public class MoveToPieceStrategyState : StrategyState
    {
        public MoveToPieceStrategyState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            conditions.Add(new IsPlayerBlocked(strategyInfo));
            conditions.Add(new IsOnFieldWithNoPieceStrategyCondition(strategyInfo));
            conditions.Add(new IsNoPieceAvailableStrategyCondition(strategyInfo));
            conditions.Add(new IsOnFieldWithPieceStrategyCondition(strategyInfo));
        }
    }
}