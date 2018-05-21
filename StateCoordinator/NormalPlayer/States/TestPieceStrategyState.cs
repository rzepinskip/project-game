using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.NormalPlayer.States
{
    public class TestPieceStrategyState : NormalPlayerStrategyState
    {
        public TestPieceStrategyState(NormalPlayerStrategyInfo playerStrategyInfo) : base(
            StateTransitionType.Triggered,
            playerStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new HasNoPieceStrategyTransition(playerStrategyInfo),
                new HasNormalPieceStrategyTransition(playerStrategyInfo),
                new HasShamStrategyTransition(playerStrategyInfo)
            };
        }
    }
}