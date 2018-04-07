using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    public class JoinGameMessage : IMessage
    {
        public JoinGameMessage(string gameName, PlayerType preferedRole, TeamColor preferedTeam)
        {
            this.GameName = gameName;
            this.PreferedRole = preferedRole;
            this.PreferedTeam = preferedTeam;
        }

        public string GameName { get; set; }
        public PlayerType PreferedRole { get; set; }
        public TeamColor PreferedTeam { get; set; }
        public IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public void Process(IPlayer player)
        {
            throw new NotImplementedException();
        }
    }
}
