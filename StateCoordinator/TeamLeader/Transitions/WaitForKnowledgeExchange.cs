using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.TeamLeader.States;

namespace PlayerStateCoordinator.TeamLeader.Transitions
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