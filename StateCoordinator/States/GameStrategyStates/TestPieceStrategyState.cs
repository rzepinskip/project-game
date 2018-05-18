using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameStrategyTransitions;

namespace PlayerStateCoordinator.States.GameStrategyStates
{
    public class TestPieceStrategyState : GameStrategyState
    {
        public TestPieceStrategyState(GameStrategyInfo gameStrategyInfo) : base(
            StateTransitionType.Triggered,
            gameStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new HasNoPieceStrategyTransition(gameStrategyInfo),
                new HasNormalPieceStrategyTransition(gameStrategyInfo),
                new HasShamStrategyTransition(gameStrategyInfo)
            };
        }
    }
}