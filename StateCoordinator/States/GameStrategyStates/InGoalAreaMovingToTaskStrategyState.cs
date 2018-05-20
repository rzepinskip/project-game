using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameStrategyTransitions;

namespace PlayerStateCoordinator.States.GameStrategyStates
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