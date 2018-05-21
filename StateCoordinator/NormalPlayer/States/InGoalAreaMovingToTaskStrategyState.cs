using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.NormalPlayer.States
{
    public class InGoalAreaMovingToTaskStrategyState : NormalPlayerStrategyState
    {
        public InGoalAreaMovingToTaskStrategyState(NormalPlayerStrategyInfo playerStrategyInfo) : base(
            StateTransitionType.Triggered,
            playerStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsPlayerBlockedStrategyTransition(playerStrategyInfo, this),
                new IsInGoalWithoutPieceStrategyTransition(playerStrategyInfo),
                new IsInTaskAreaStrategyTransition(playerStrategyInfo)
            };
        }
    }
}