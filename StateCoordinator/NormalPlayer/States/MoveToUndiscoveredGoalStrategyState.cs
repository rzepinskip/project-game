using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.NormalPlayer.States
{
    public class MoveToUndiscoveredGoalStrategyState : NormalPlayerStrategyState
    {
        public MoveToUndiscoveredGoalStrategyState(GameStrategyInfo gameStrategyInfo) : base(
            StateTransitionType.Triggered,
            gameStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsPlayerBlockedStrategyTransition(gameStrategyInfo, this),
                new IsOnDesiredUndiscoveredGoalFieldStrategyTransition(gameStrategyInfo),
                new HasNormalPieceStrategyTransition(gameStrategyInfo)
            };
        }
    }
}