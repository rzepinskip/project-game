using System.Collections.Generic;
using Common.Interfaces;
using Messaging.InitializationMessages;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GameInitialization.States;
using PlayerStateCoordinator.Info;

namespace PlayerStateCoordinator.GameInitialization.Transitions
{
    public class GetGamesTransition : GameInitializationTransition
    {
        public GetGamesTransition(GameInitializationInfo gameInitializationInfo) : base(gameInitializationInfo)
        {
        }

        public override State NextState => new MatchingGameState(GameInitializationInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage> {new GetGamesMessage()};

        public override bool IsPossible()
        {
            return true;
        }
    }
}