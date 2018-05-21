using System.Collections.Generic;
using System.Linq;
using Common.Interfaces;
using Messaging.InitializationMessages;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GameInitialization.States;

namespace PlayerStateCoordinator.GameInitialization.Transitions
{
    public class MatchingGameTransition : GameInitializationTransition
    {
        public MatchingGameTransition(GameInitializationInfo gameInitializationInfo) : base(gameInitializationInfo)
        {
        }

        public override State NextState => new AwaitingJoinResponseState(GameInitializationInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage>
        {
            new JoinGameMessage(GameInitializationInfo.GameName, GameInitializationInfo.Role,
                GameInitializationInfo.Team)
        };

        public override bool IsPossible()
        {
            if (GameInitializationInfo.GamesInfo == null)
                return false;
            return GameInitializationInfo.GamesInfo.Any(x => x.GameName == GameInitializationInfo.GameName);
        }
    }
}