using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameStrategyTransitions;

namespace PlayerStateCoordinator.States.GameStrategyStates
{
    public class InitStrategyState : NormalPlayerStrategyState
    {
        public InitStrategyState(GameStrategyInfo gameStrategyInfo) : base(
            StateTransitionType.Immediate,
            gameStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsInGoalWithoutPieceStrategyTransition(gameStrategyInfo),
                new IsInTaskAreaStrategyTransition(gameStrategyInfo)
            };
        }
    }
}