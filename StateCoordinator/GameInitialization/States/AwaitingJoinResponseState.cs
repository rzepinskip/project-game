using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.GameInitialization.Transitions;

namespace PlayerStateCoordinator.GameInitialization.States
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