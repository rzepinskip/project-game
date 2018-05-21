using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.NormalPlayer.States
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