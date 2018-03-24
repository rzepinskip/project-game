using Player.Strategy.States;
using Shared.GameMessages;
using Shared.GameMessages.PieceActions;

namespace Player.Strategy.Conditions
{
    public class IsAbleToTestCondition : Condition
    {
        public IsAbleToTestCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            return true;
        }

        public override State GetNextState(State fromState)
        {
            return new TestPieceState(StrategyInfo);
        }

        public override GameMessage GetNextMessage(State fromState)
        {
            return new TestPiece
            {
                PlayerId = StrategyInfo.PlayerId
            };
        }
    }
}