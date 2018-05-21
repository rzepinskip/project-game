using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.GamePlay.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer.States
{
    public class InitStrategyState : NormalPlayerStrategyState
    {
        public InitStrategyState(NormalPlayerStrategyInfo playerStrategyInfo) : base(
            StateTransitionType.Immediate,
            playerStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsInGoalWithoutPieceStrategyTransition(playerStrategyInfo),
                new IsInTaskAreaStrategyTransition(playerStrategyInfo)
            };
        }
    }
}