using System;
using Common;

namespace Messaging.ActionHelpers
{
    public class ActionLog
    {
        public ActionLog(int playerId, int gameId, string playerGuid, PlayerInfo playerInfo,
            ActionType actionType)
        {
            ActionType = actionType;
            Timestamp = DateTime.Now;
            GameID = gameId;
            PlayerID = playerId;
            PlayerGUID = playerGuid;
            Colour = playerInfo.Team;
            Role = playerInfo.Role;
        }

        public ActionType ActionType { get; set; }
        public DateTime Timestamp { get; set; }
        public int GameID { get; set; }
        public int PlayerID { get; set; }
        public string PlayerGUID { get; set; }
        public TeamColor Colour { get; set; }
        public PlayerType Role { get; set; }

        public override string ToString()
        {
            return string.Join('\t', ActionType, Timestamp, GameID, PlayerID, PlayerGUID, Colour, Role);
        }
    }
}