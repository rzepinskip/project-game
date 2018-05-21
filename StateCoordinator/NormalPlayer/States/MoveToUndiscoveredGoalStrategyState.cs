using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.NormalPlayer.States
{
    public class MoveToUndiscoveredGoalStrategyState : NormalPlayerStrategyState
    {
        public MoveToUndiscoveredGoalStrategyState(NormalPlayerStrategyInfo playerStrategyInfo) : base(
            StateTransitionType.Triggered,
            playerStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsPlayerBlockedStrategyTransition(playerStrategyInfo, this),
                new IsOnDesiredUndiscoveredGoalFieldStrategyTransition(playerStrategyInfo),
                new HasNormalPieceStrategyTransition(playerStrategyInfo)
            };
        }
    }
}