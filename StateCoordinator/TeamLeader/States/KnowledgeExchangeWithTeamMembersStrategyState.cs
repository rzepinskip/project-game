using System;
using System.Collections.Generic;
using Common.Interfaces;
using Messaging;
using Messaging.KnowledgeExchangeMessages;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.Transitions;

namespace PlayerStateCoordinator.TeamLeader.States
{
    public class KnowledgeExchangeWithTeamMembersStrategyState : GamePlayStrategyState
    {
        public KnowledgeExchangeWithTeamMembersStrategyState(GameStrategyInfo gameStrategyInfo) : base(StateTransitionType.Triggered, gameStrategyInfo)
        {
            Transitions = new Transition[0];
        }
        protected virtual bool IsExchangeWantedWithPlayer(int initiatorId)
        {
            return GameStrategyInfo.Board.Players[initiatorId].Team == GameStrategyInfo.Team;
        }
    }
}