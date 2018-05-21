using System;
using System.Collections.Generic;
using ClientsCommon.ActionAvailability.AvailabilityChain;
using Common;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.States;
using PlayerStateCoordinator.TeamLeader;

namespace PlayerStateCoordinator.NormalPlayer.Transitions
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