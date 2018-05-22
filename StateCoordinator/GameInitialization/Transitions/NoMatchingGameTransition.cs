using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Interfaces;
using Messaging;
using Messaging.InitializationMessages;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GameInitialization.States;

namespace PlayerStateCoordinator.GameInitialization.Transitions
{
    public class NoMatchingGameTransition : GameInitializationTransition
    {
        public NoMatchingGameTransition(GameInitializationInfo gameInitializationInfo) : base(gameInitializationInfo)
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
            if (GameInitializationInfo.GamesInfo == null)
                return true;

            return GameInitializationInfo.GamesInfo.All(x => x.GameName != GameInitializationInfo.GameName);
        }
    }
}