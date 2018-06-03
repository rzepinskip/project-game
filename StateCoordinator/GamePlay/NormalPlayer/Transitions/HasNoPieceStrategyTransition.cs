using System.Collections.Generic;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GamePlay.NormalPlayer.States;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer.Transitions
{
    public class HasNoPieceStrategyTransition : NormalPlayerStrategyTransition
    {
        public HasNoPieceStrategyTransition(NormalPlayerStrategyInfo normalPlayerStrategyInfo) : base(
            normalPlayerStrategyInfo)
        {
        }

        public override State NextState => new DiscoverStrategyState(NormalPlayerStrategyInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage>
        {
            new DiscoverRequest(NormalPlayerStrategyInfo.PlayerGuid, NormalPlayerStrategyInfo.GameId)
        };

        public override bool IsPossible()
        {
            var playerInfo = NormalPlayerStrategyInfo.Board.Players[NormalPlayerStrategyInfo.PlayerId];
            return playerInfo.Piece == null;
        }
    }
}