using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GameInitialization.States;

namespace PlayerStateCoordinator.GameInitialization.Transitions
{
    public class JoinSuccessfulTransition : GameInitializationTransition
    {
        public JoinSuccessfulTransition(GameInitializationInfo gameInitializationInfo) : base(gameInitializationInfo)
        {
        }

        public override State NextState => new GameStartedState(GameInitializationInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage>();

        public override bool IsPossible()
        {
            return GameInitializationInfo.JoiningSuccessful;
        }
    }
}