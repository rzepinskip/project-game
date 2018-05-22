using System.Collections.Generic;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GamePlay.NormalPlayer.States;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer.Transitions
{
    public class IsInTaskAreaStrategyTransition : NormalPlayerStrategyTransition
    {
        public IsInTaskAreaStrategyTransition(NormalPlayerStrategyInfo normalPlayerStrategyInfo) : base(
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
            var currentLocation = NormalPlayerStrategyInfo.CurrentLocation;
            return NormalPlayerStrategyInfo.Board.IsLocationInTaskArea(currentLocation);
        }
    }
}