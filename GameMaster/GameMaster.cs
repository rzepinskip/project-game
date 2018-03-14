using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;

namespace GameMaster
{
    class GameMaster
    {
        Dictionary<string, int> PlayerGuidToId { get; }
        Board Board { get; set; }

        //private ActionLog GetLog(GameMessage m)
        //{
        //    var playerId = PlayerGuidToId[m.PlayerGuid];
        //    var playerInfo = Board.Players[playerId];

        //    return new ActionLog(playerId, playerInfo, m);
        //}

        private void PutLog(string filename, ActionLog log)
        {
            if (!File.Exists(filename))
            {
                string logsHeader = string.Join('\t', "Type", "Timestamp", "Game ID",
                    "Player ID", "Player GUID", "Colour", "Role");
                File.WriteAllText(filename, logsHeader);
            }

            File.WriteAllText(filename, log.ToString());
        }
    }
}
