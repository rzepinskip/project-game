using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GameInitialization.States;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.GameInitialization;

namespace PlayerStateCoordinator.GameInitialization.Transitions
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