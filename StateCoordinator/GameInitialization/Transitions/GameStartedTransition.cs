using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Info;

namespace PlayerStateCoordinator.GameInitialization.Transitions
{
    public class GameStartedTransition : GameInitializationTransition
    {
        public GameStartedTransition(GameInitializationInfo gameInitializationInfo)
            : base(gameInitializationInfo)
        {
        }

        public override State NextState => GameInitializationInfo.PlayerGameStrategyBeginningState;

        public override IEnumerable<IMessage> Message => new List<IMessage>();

        public override bool IsPossible()
        {
            return GameInitializationInfo.IsGameRunning;
        }
    }
}