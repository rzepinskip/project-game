using System.Collections.Generic;
using System.Linq;
using Common.Interfaces;
using Messaging.InitializationMessages;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.States.InitializationStates;

namespace PlayerStateCoordinator.Transitions.GameInitializationTransitions
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
                GameInitializationInfo.Color)
        };

        public override bool IsPossible()
        {
            if (GameInitializationInfo.GamesInfo == null)
                return false;
            return GameInitializationInfo.GamesInfo.Any(x => x.GameName == GameInitializationInfo.GameName);
        }
    }
}