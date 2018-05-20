using PlayerStateCoordinator.Info;

namespace PlayerStateCoordinator.States
{
    public class NormalPlayerStrategyState : GamePlayStrategyState
    {
        protected readonly GameStrategyInfo GameStrategyInfo;

        public NormalPlayerStrategyState(StateTransitionType transitionType,
            GameStrategyInfo gameStrategyInfo) : base(transitionType, gameStrategyInfo)
        {
            GameStrategyInfo = gameStrategyInfo;
        }
    }
}