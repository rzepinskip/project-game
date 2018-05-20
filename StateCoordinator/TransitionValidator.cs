using Common;
using Common.BoardObjects;
using Common.Interfaces;
using PlayerStateCoordinator.Info;

namespace PlayerStateCoordinator
{
    public class TransitionValidator
    {
        public static bool IsFarFromEnemyGoalArea(TeamColor team, BoardBase board, Location currentLocation)
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