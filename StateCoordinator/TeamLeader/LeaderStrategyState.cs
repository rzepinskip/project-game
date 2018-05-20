using System;
using System.Collections.Generic;
using Common.Interfaces;
using Messaging;
using Messaging.KnowledgeExchangeMessages;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.Transitions;

namespace PlayerStateCoordinator.TeamLeader
{
    public class LeaderStrategyState : GamePlayStrategyState
    {
        public LeaderStrategyState(StateTransitionType transitionType,
            GameStrategyInfo gameStrategyInfo) : base(transitionType, gameStrategyInfo)
        {
        }

        protected override bool IsExchangeWantedWithPlayer(int initiatorId)
        {
            return false;
        }

    }
}