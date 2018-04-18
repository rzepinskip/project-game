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

        public override StrategyState GetNextState(StrategyState fromStrategyState)
        {
            return new DiscoverStrategyState(StrategyInfo);
        }

        public override Request GetNextMessage(StrategyState fromStrategyState)
        {
            return new DiscoverRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId);
        }
    }
}