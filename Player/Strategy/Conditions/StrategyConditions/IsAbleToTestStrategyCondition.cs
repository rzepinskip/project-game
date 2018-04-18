using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.StrategyConditions
{
    public class IsAbleToTestStrategyCondition : StrategyCondition
    {
        public IsAbleToTestStrategyCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            return true;
        }

        public override StrategyState GetNextState(StrategyState fromStrategyState)
        {
            return new TestPieceStrategyState(StrategyInfo);
        }

        public override Request GetNextMessage(StrategyState fromStrategyState)
        {
            return new TestPieceRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId);
        }
    }
}