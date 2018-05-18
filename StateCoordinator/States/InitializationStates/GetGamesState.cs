using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameInitializationTransitions;

namespace PlayerStateCoordinator.States.InitializationStates
{
    public class GetGamesState : GameInitializationState
    {
        public GetGamesState(GameInitializationInfo baseInfo) : base(
            StateTransitionType.Triggered,
            new Transition[]
            {
                new GetGamesTransition(baseInfo)
            },
            baseInfo)
        {
        }
    }
}