using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.GameInitialization.Transitions;
using PlayerStateCoordinator.Info;

namespace PlayerStateCoordinator.GameInitialization.States
{
    public class GetGamesState : GameInitializationState
    {
        public GetGamesState(GameInitializationInfo baseInfo) : base(
            StateTransitionType.Triggered,
            baseInfo)
        {
            Transitions = new Transition[]
            {
                new GetGamesTransition(baseInfo)
            };
        }
    }
}