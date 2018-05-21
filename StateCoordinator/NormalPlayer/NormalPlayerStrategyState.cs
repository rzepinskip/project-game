using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;

namespace PlayerStateCoordinator.NormalPlayer
{
    public class NormalPlayerStrategyState : GamePlayStrategyState
    {
        public NormalPlayerStrategyState(StateTransitionType transitionType,
            GameStrategyInfo gameStrategyInfo) : base(transitionType, gameStrategyInfo)
        {
        }
    }
}