using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.NormalPlayer.States
{
    public class ReportGoalFieldsStrategyState : NormalPlayerStrategyState
    {
        public ReportGoalFieldsStrategyState(NormalPlayerStrategyInfo playerStrategyInfo) : base(
            StateTransitionType.Triggered,
            playerStrategyInfo)
        {
            Transitions = new Transition[]
            {
                new IsThereSomeoneToCommunicateWithStrategyTransition(playerStrategyInfo),
                new IsInGoalWithoutPieceStrategyTransition(playerStrategyInfo)
            };
        }
    }
}