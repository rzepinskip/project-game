using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.Transitions;

namespace PlayerStateCoordinator.NormalPlayer.States
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