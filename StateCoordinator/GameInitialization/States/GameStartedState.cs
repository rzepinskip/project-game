using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.GameInitialization.Transitions;

namespace PlayerStateCoordinator.GameInitialization.States
{
    public class GameStartedState : GameInitializationState
    {
        public GameStartedState(GameInitializationInfo baseInfo) : base(
            StateTransitionType.Triggered,
            baseInfo)
        {
            baseInfo.IsGameRunning = true;
            Transitions = new Transition[]
            {
                new GameStartedTransition(baseInfo),
                new GameEndedTransition(baseInfo)
            };
        }
    }
}