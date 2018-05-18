using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameInitializationTransitions;

namespace PlayerStateCoordinator.States.InitializationStates
{
    public class MatchingGameState : GameInitializationState
    {
        public MatchingGameState(GameInitializationInfo baseInfo) : base(
            StateTransitionType.Triggered,
            new Transition[]
            {
                new MatchingGameTransition(baseInfo),
                new NoMatchingGameTransition(baseInfo)
            },
            baseInfo)
        {
        }
    }
}