using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.TeamLeader.States;

namespace PlayerStateCoordinator.TeamLeader.Transitions
{
    public class NearEnemyGoalAreaTransition : LeaderStrategyTransition
    {
        public NearEnemyGoalAreaTransition(LeaderStrategyInfo leaderStrategyInfo) : base(leaderStrategyInfo)
        {
        }

        public override State NextState => new KnowledgeExchangeWithTeamMembersStrategyState(LeaderStrategyInfo);
        public override IEnumerable<IMessage> Message => new List<IMessage>();

        public override bool IsPossible()
        {
            return !TransitionValidator.IsFarFromEnemyGoalArea(LeaderStrategyInfo.Team, LeaderStrategyInfo.Board,
                LeaderStrategyInfo.CurrentLocation);
        }
    }
}