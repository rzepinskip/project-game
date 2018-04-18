using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.StrategyConditions
{
    public class IsInTaskAreaStrategyCondition : StrategyCondition
    {
        public IsInTaskAreaStrategyCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            return StrategyInfo.Board.IsLocationInTaskArea(StrategyInfo.FromLocation);
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new DiscoverStrategyState(StrategyInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
        {
            return new DiscoverRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId);
        }
    }
}