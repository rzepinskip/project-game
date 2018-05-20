using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.States.GameInitializationStates;

namespace PlayerStateCoordinator.Transitions.GameInitializationTransitions
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