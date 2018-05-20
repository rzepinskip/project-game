using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameInitializationTransitions;

namespace PlayerStateCoordinator.States.GameInitializationStates
{
    public class AwaitingJoinResponseState : GameInitializationState
    {
        public AwaitingJoinResponseState(GameInitializationInfo baseInfo) : base(
            StateTransitionType.Triggered,
            baseInfo)
        {
            Transitions = new Transition[]
            {
                new JoinSuccessfulTransition(baseInfo),
                new JoinUnsuccessfulTransition(baseInfo)
            };
        }
    }
}