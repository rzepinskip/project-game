using System;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.Info;

namespace PlayerStateCoordinator.TeamLeader.States
{
    public class KnowledgeExchangeWithTeamMembersStrategyState : LeaderStrategyState
    {
        public KnowledgeExchangeWithTeamMembersStrategyState(GameStrategyInfo gameStrategyInfo) : base(
            StateTransitionType.Triggered, gameStrategyInfo)
        {
            Transitions = new Transition[0];
        }

        protected override bool IsExchangeWantedWithPlayer(int initiatorId)
        {
            bool isExchangeWanted = GameStrategyInfo.Board.Players[initiatorId].Team == GameStrategyInfo.Team;
            Console.WriteLine($" {GameStrategyInfo.Team} {GameStrategyInfo.PlayerId} exchange for initiator {initiatorId} {GameStrategyInfo.Board.Players[initiatorId].Team} wanted {isExchangeWanted}");
            return isExchangeWanted;
        }
    }
}