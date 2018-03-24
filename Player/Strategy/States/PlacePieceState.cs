using Player.Strategy.Conditions;

namespace Player.Strategy.States
{
    public class PlacePieceState : State
    {
        protected PlacePieceState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            conditions.Add(new IsInGoalWithoutPieceCondition(strategyInfo));
        }
    }
}