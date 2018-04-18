using Player.Strategy.Conditions.StrategyConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.StrategyStates
{
    public class MoveToPieceStrategyState : StrategyState
    {
        public MoveToPieceStrategyState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            transitionConditions.Add(new IsPlayerBlocked(strategyInfo));
            transitionConditions.Add(new IsOnFieldWithNoPieceStrategyCondition(strategyInfo));
            transitionConditions.Add(new IsNoPieceAvailableStrategyCondition(strategyInfo));
            transitionConditions.Add(new IsOnFieldWithPieceStrategyCondition(strategyInfo));
        }
    }
}