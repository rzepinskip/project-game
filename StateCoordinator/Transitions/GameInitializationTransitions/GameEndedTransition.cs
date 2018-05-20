using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.States.GameInitializationStates;

namespace PlayerStateCoordinator.Transitions.GameInitializationTransitions
{
    public class GameEndedTransition : GameInitializationTransition
    {
        public GameEndedTransition(GameInitializationInfo gameInitializationInfo)
            : base(gameInitializationInfo)
        {
        }

        public override State NextState => new GetGamesState(GameInitializationInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage>();

        public override bool IsPossible()
        {
            return !GameInitializationInfo.IsGameRunning;
        }
    }
}