using System;

namespace Shared
{
    public class ActionLog
    {
        public ActionLog(int playerId, int gameId, string playerGuid, PlayerInfo playerInfo,
            CommonResources.ActionType actionType)
        {
            ActionType = actionType;
            Timestamp = DateTime.Now;
            GameID = gameId;
            PlayerID = playerId;
            PlayerGUID = playerGuid;
            Colour = playerInfo.Team;
            Role = playerInfo.Role;
        }

        public CommonResources.ActionType ActionType { get; set; }
        public DateTime Timestamp { get; set; }
        public int GameID { get; set; }
        public int PlayerID { get; set; }
        public string PlayerGUID { get; set; }
        public CommonResources.TeamColour Colour { get; set; }
        public PlayerBase.PlayerType Role { get; set; }

        public override string ToString()
        {
            return string.Join('\t', ActionType, Timestamp, GameID, PlayerID, PlayerGUID, Colour, Role);
        }
    }
}