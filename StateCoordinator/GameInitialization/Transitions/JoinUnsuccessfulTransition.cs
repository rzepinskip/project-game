using System.Collections.Generic;
using System.Threading;
using Common.Interfaces;
using Messaging;
using Messaging.InitializationMessages;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GameInitialization.States;

namespace PlayerStateCoordinator.GameInitialization.Transitions
{
    public class JoinUnsuccessfulTransition : GameInitializationTransition
    {
        public JoinUnsuccessfulTransition(GameInitializationInfo gameInitializationInfo) : base(gameInitializationInfo)
        {
        }

        public override State NextState => new MatchingGameState(GameInitializationInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                Thread.Sleep(Constants.DefaultRequestRetryInterval);
                return new List<IMessage> {new GetGamesMessage()};
            }
        }

        public override bool IsPossible()
        {
            return !GameInitializationInfo.JoiningSuccessful;
        }
    }
}