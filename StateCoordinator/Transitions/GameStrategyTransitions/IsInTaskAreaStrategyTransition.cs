using System.Collections.Generic;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.States.GameStrategyStates;

namespace PlayerStateCoordinator.Transitions.GameStrategyTransitions
{
    public class IsInTaskAreaStrategyTransition : GameStrategyTransition
    {
        public IsInTaskAreaStrategyTransition(GameStrategyInfo gameStrategyInfo) : base(gameStrategyInfo)
        {
        }

        public override State NextState => new DiscoverStrategyState(GameStrategyInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage>
        {
            new DiscoverRequest(GameStrategyInfo.PlayerGuid, GameStrategyInfo.GameId)
        };

        public override bool IsPossible()
        {
            var currentLocation = GameStrategyInfo.CurrentLocation;
            return GameStrategyInfo.Board.IsLocationInTaskArea(currentLocation);
        }
    }
}
