using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.NormalPlayer.States
{
    public class TestPieceStrategyState : NormalPlayerStrategyState
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