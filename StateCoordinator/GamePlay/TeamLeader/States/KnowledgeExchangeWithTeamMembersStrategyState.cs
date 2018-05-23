using System;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.Transitions;

namespace PlayerStateCoordinator.GamePlay.TeamLeader.States
{
    public class KnowledgeExchangeWithTeamMembersStrategyState : LeaderStrategyState
    {
        public KnowledgeExchangeWithTeamMembersStrategyState(LeaderStrategyInfo leaderStrategyInfo) : base(
            StateTransitionType.Triggered, leaderStrategyInfo)
        {
            Transitions = new Transition[0];
        }

        protected override bool IsExchangeWantedWithPlayer(int initiatorId)
        {
            var isExchangeWanted = PlayerStrategyInfo.Board.Players[initiatorId].Team == PlayerStrategyInfo.Team;
            //Console.WriteLine(
            //    $" {PlayerStrategyInfo.Team} {PlayerStrategyInfo.PlayerId} exchange for initiator {initiatorId} {PlayerStrategyInfo.Board.Players[initiatorId].Team} wanted {isExchangeWanted}");
            return isExchangeWanted;
        }
    }
}