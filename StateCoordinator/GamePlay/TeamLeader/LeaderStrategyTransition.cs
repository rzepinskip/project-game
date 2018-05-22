using Common;
using Common.BoardObjects;

namespace PlayerStateCoordinator.GamePlay.TeamLeader
{
    public abstract class LeaderStrategyTransition : GameStrategyTransition
    {
        protected LeaderStrategyInfo LeaderStrategyInfo;

        protected LeaderStrategyTransition(LeaderStrategyInfo leaderStrategyInfo) : base(leaderStrategyInfo)
        {
            LeaderStrategyInfo = leaderStrategyInfo;
        }
        protected bool IsFarFromEnemyGoalArea(TeamColor team, BoardBase board, Location currentLocation)
        {
            var enemyTeamColor = team == TeamColor.Blue ? TeamColor.Red : TeamColor.Blue;
            var enemyGoalAreaStartY = board.GoalAreaStartYFor(enemyTeamColor);
            var isBottomTeam = team == TeamColor.Blue;
            return isBottomTeam
                ? currentLocation.Y + 1 < enemyGoalAreaStartY
                : currentLocation.Y - 1 > enemyGoalAreaStartY;
        }
    }
}