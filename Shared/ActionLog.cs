using System;
using System.Collections.Generic;
using System.Text;
using Shared;
using Shared.GameMessages;

namespace Shared
{
    public class ActionLog
    {
        //public ActionLog(int playerId, PlayerInfo playerInfo, GameMessage m)
        //{
        //    ActionType = m.GetActionType();
        //    Timestamp = DateTime.Now;
        //    GameID = m.GameId;
        //    PlayerID = playerId;
        //    PlayerGUID = m.PlayerGuid;
        //    Colour = playerInfo.Team;
        //    Role = playerInfo.Role;
        //}

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
