using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.NormalPlayer.States
{
    public class InGoalAreaMovingToTaskStrategyState : NormalPlayerStrategyState
    {
        public InGoalAreaMovingToTaskStrategyState(GameStrategyInfo gameStrategyInfo) : base(
            StateTransitionType.Triggered,
            gameStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsPlayerBlockedStrategyTransition(gameStrategyInfo, this),
                new IsInGoalWithoutPieceStrategyTransition(gameStrategyInfo),
                new IsInTaskAreaStrategyTransition(gameStrategyInfo)
            };
        }
    }
}