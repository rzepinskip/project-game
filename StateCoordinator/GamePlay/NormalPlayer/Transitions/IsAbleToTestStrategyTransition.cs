using System.Collections.Generic;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GamePlay.NormalPlayer.States;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer.Transitions
{
    public class IsAbleToTestStrategyTransition : NormalPlayerStrategyTransition
    {
        public IsAbleToTestStrategyTransition(NormalPlayerStrategyInfo normalPlayerStrategyInfo) : base(
            normalPlayerStrategyInfo)
        {
        }

        public override State NextState => new TestPieceStrategyState(NormalPlayerStrategyInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage>
        {
            new TestPieceRequest(NormalPlayerStrategyInfo.PlayerGuid, NormalPlayerStrategyInfo.GameId)
        };

        public override bool IsPossible()
        {
            return true;
        }
    }
}