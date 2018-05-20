using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameInitializationTransitions;

namespace PlayerStateCoordinator.States.GameInitializationStates
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