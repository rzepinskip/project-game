using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.GamePlay.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer.States
{
    public class MoveToUndiscoveredGoalStrategyState : NormalPlayerStrategyState
    {
        public MoveToUndiscoveredGoalStrategyState(NormalPlayerStrategyInfo playerStrategyInfo) : base(
            StateTransitionType.Triggered,
            playerStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsNormalPlayerBlockedTransition(playerStrategyInfo, this),
                new IsOnDesiredUndiscoveredGoalFieldStrategyTransition(playerStrategyInfo),
                new HasNormalPieceStrategyTransition(playerStrategyInfo)
            };
        }
    }
}