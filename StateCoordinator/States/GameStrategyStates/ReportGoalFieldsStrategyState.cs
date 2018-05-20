using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.Transitions;
using PlayerStateCoordinator.Transitions.GameStrategyTransitions;

namespace PlayerStateCoordinator.States.GameStrategyStates
{
    public class ReportGoalFieldsStrategyState : NormalPlayerStrategyState
    {
        public ReportGoalFieldsStrategyState(GameStrategyInfo gameStrategyInfo) : base(
            StateTransitionType.Triggered,
            gameStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsThereSomeoneToCommunicateWithStrategyTransition(gameStrategyInfo),
                new IsInGoalWithoutPieceStrategyTransition(gameStrategyInfo)
            };
        }
    }
}