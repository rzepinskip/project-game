using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameStrategyTransitions;

namespace PlayerStateCoordinator.States.GameStrategyStates
{
    public class DestroyPieceStrategyState : GameStrategyState
    {
        public DestroyPieceStrategyState(GameStrategyInfo gameStrategyInfo) : base(
            StateTransitionType.Triggered,
            new Transition[]
            {
                new HasNoPieceTransition(gameStrategyInfo)
            },
            gameStrategyInfo)
        {
        }
    }
}