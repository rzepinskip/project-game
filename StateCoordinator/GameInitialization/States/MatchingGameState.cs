using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.GameInitialization.Transitions;
using PlayerStateCoordinator.Info;

namespace PlayerStateCoordinator.GameInitialization.States
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