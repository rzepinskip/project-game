using System;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.GamePlay.NormalPlayer.States;

namespace PlayerStateCoordinator.GamePlay.NormalPlayer.Transitions
{
    public class IsNormalPlayerBlockedTransition : IsPlayerBlockedTransition
    {
        private readonly NormalPlayerStrategyInfo _normalPlayerStrategyInfo;
        public IsNormalPlayerBlockedTransition(NormalPlayerStrategyInfo gamePlayStrategyInfo, NormalPlayerStrategyState fromState) : base(gamePlayStrategyInfo, fromState)
        {
            _normalPlayerStrategyInfo = gamePlayStrategyInfo;
        }

        protected override GamePlayStrategyState GetRecoveryFromBlockedState()
        {
            return new DiscoverStrategyState(_normalPlayerStrategyInfo);
        }

        protected override GamePlayStrategyState GetFromState()
        {
            return Activator.CreateInstance(FromState.GetType(),
                GamePlayStrategyInfo) as NormalPlayerStrategyState;
        }
    }
}