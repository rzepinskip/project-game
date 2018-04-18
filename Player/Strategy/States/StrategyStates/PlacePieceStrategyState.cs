using Player.Strategy.Conditions.StrategyConditions;
using Player.Strategy.StateInfo;

namespace Player.Strategy.States.StrategyStates
{
    public class PlacePieceStrategyState : StrategyState
    {
        protected PlacePieceStrategyState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            conditions.Add(new IsInGoalWithoutPieceStrategyCondition(strategyInfo));
        }
    }
}