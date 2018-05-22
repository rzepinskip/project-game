using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GamePlay.TeamLeader.States;

namespace PlayerStateCoordinator.GamePlay.TeamLeader.Transitions
{
    public class WaitForKnowledgeExchange : LeaderStrategyTransition
    {
        public WaitForKnowledgeExchange(LeaderStrategyInfo leaderStrategyInfo) : base(leaderStrategyInfo)
        {
        }

        public override State NextState => new KnowledgeExchangeWithTeamMembersStrategyState(LeaderStrategyInfo);

        public override IEnumerable<IMessage> Message => new List<IMessage>();

        public override bool IsPossible()
        {
            return true;
        }
    }
}