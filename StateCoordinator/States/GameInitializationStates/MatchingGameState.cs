using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameInitializationTransitions;

namespace PlayerStateCoordinator.States.GameInitializationStates
{
    public class MatchingGameState : GameInitializationState
    {
        public MatchingGameState(GameInitializationInfo baseInfo) : base(
            StateTransitionType.Triggered,
            baseInfo)
        {
            Transitions = new Transition[]
            {
                new MatchingGameTransition(baseInfo),
                new NoMatchingGameTransition(baseInfo)
            };
        }
    }
}