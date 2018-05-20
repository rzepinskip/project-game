using System.Collections.Generic;
using Common.Interfaces;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.Info;
using PlayerStateCoordinator.TeamLeader.States;

namespace PlayerStateCoordinator.TeamLeader.Transitions
{
    public class NearEnemyGoalAreaTransition : GameStrategyTransition
    {
        public NearEnemyGoalAreaTransition(GameStrategyInfo gameStrategyInfo) : base(gameStrategyInfo)
        {
        }

        public override State NextState => new KnowledgeExchangeWithTeamMembersStrategyState(GameStrategyInfo);
        public override IEnumerable<IMessage> Message => new List<IMessage>();

        public override bool IsPossible()
        {
            return !TransitionValidator.IsFarFromEnemyGoalArea(GameStrategyInfo.Team, GameStrategyInfo.Board,
                GameStrategyInfo.CurrentLocation);
        }
    }
}