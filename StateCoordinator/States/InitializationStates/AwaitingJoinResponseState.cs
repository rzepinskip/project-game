using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameInitializationTransitions;

namespace PlayerStateCoordinator.States.InitializationStates
{
    public class AwaitingJoinResponseState : GameInitializationState
    {
        public AwaitingJoinResponseState(GameInitializationInfo baseInfo) : base(
            StateTransitionType.Triggered,
            new Transition[]
            {
                new JoinSuccessfulTransition(baseInfo),
                new JoinUnsuccessfulTransition(baseInfo)
            },
            baseInfo)
        {
        }
    }
}