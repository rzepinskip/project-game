using System.Collections.Generic;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.NormalPlayer.States;

namespace PlayerStateCoordinator.NormalPlayer.Transitions
{
    public class IsAbleToTestStrategyTransition : GameStrategyTransition
    {
        public IsAbleToTestStrategyTransition(GameStrategyInfo gameStrategyInfo) : base(gameStrategyInfo)
        {
        }

        public override State NextState => new TestPieceStrategyState(GameStrategyInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage>
        {
            new TestPieceRequest(GameStrategyInfo.PlayerGuid, GameStrategyInfo.GameId)
        };

        public override bool IsPossible()
        {
            return true;
        }
    }
}