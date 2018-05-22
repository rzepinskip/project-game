using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.GamePlay.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer.States
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