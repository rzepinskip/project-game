using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.StrategyConditions
{
    public class HasNoPieceStrategyCondition : StrategyCondition
    {
        public HasNoPieceStrategyCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            var playerInfo = StrategyInfo.Board.Players[StrategyInfo.PlayerId];
            return playerInfo.Piece == null;
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