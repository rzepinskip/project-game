using Player.Strategy.Conditions;

namespace Player.Strategy.States
{
    public class PickupPieceState : State
    {
        public PickupPieceState(StrategyInfo strategyInfo) : base(strategyInfo)
        {
            conditions.Add(new IsAbleToTestCondition(strategyInfo));
        }
    }
}