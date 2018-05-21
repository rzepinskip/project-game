using System.Collections.Generic;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.States;

namespace PlayerStateCoordinator.NormalPlayer.Transitions
{
    public class IsInTaskAreaStrategyTransition : NormalPlayerStrategyTransition
    {
        public IsInTaskAreaStrategyTransition(NormalPlayerStrategyInfo normalPlayerStrategyInfo) : base(normalPlayerStrategyInfo)
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