using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace Player.Strategy
{
    public class StrategyInfo : ILoggable
    {
        public StrategyInfo(Location fromLocation, BoardBase board, string playerGuid, int gameId, PlayerBase player,
            List<GoalField> undiscoveredGoalFields = null, Location toLocation = null)
        {
            FromLocation = fromLocation;
            ToLocation = toLocation;
            Board = board;
            PlayerId = player.Id;
            PlayerGuid = playerGuid;
            GameId = gameId;
            Team = player.Team;
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
            GameId = strategyInfo.GameId;
        }

        public Location FromLocation { get; set; }
        public Location ToLocation { get; set; }
        public BoardBase Board { get; set; }
        public int PlayerId { get; set; }
        public string PlayerGuid { get; set; }
        public TeamColor Team { get; set; }
        public List<GoalField> UndiscoveredGoalFields { get; set; }
        public int GameId { get; set; }

        public override string ToString()
        {
            var fields = string.Join("\n" + "\t", "\tFromLocation: " + FromLocation, "ToLocation: " + ToLocation,
                "PlayerId: " + PlayerId, "Team: " + Team, "UndiscoveredGoals: " + UndiscoveredGoalFields);

            return string.Join('\n', "Strategy Info:", fields);
        }

        public string ToLog()
        {
            return this.ToString();
        }
    }
}