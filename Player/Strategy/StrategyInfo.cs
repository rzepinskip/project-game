using System.Collections.Generic;
using Common;
using Common.BoardObjects;

namespace Player.Strategy
{
    public class StrategyInfo
    {
        public StrategyInfo(Location fromLocation, BoardBase board, int playerId, TeamColor team,
            List<GoalField> undiscoveredGoalFields = null, Location toLocation = null)
        {
            FromLocation = fromLocation;
            ToLocation = toLocation;
            Board = board;
            PlayerId = playerId;
            Team = team;
            UndiscoveredGoalFields = undiscoveredGoalFields;
        }

        public StrategyInfo(StrategyInfo strategyInfo)
        {
            FromLocation = strategyInfo.FromLocation;
            ToLocation = strategyInfo.ToLocation;
            Board = strategyInfo.Board;
            PlayerId = strategyInfo.PlayerId;
            Team = strategyInfo.Team;
            UndiscoveredGoalFields = strategyInfo.UndiscoveredGoalFields;
        }

        public Location FromLocation { get; set; }
        public Location ToLocation { get; set; }
        public BoardBase Board { get; set; }
        public int PlayerId { get; set; }
        public TeamColor Team { get; set; }
        public List<GoalField> UndiscoveredGoalFields { get; set; }

        public override string ToString()
        {
            var fields = string.Join("\n" + "\t", "\tFromLocation: " + FromLocation, "ToLocation: " + ToLocation,
                "PlayerId: " + PlayerId, "Team: " + Team, "UndiscoveredGoals: " + UndiscoveredGoalFields);

            return string.Join('\n', "Strategy Info:", fields);
        }
    }
}