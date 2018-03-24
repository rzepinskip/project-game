using Messaging.Requests;
using Player.Strategy.States;

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

        public override Request GetNextMessage(State fromState)
        {
            return new TestPieceRequest(StrategyInfo.PlayerId);
        }
    }
}