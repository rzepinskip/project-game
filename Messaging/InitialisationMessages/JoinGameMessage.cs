using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Common.ActionInfo;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    public class JoinGameMessage : IRequest
    {
        public JoinGameMessage(string gameName, PlayerType preferedRole, TeamColor preferedTeam)
        {
            this.GameName = gameName;
            this.PreferedRole = preferedRole;
            this.PreferedTeam = preferedTeam;
        }

        public int PlayerId;
        public string GameName { get; set; }
        public PlayerType PreferedRole { get; set; }
        public TeamColor PreferedTeam { get; set; }
        public IMessage Process(IGameMaster gameMaster)
        {
            //handle join game message in dispatcher handler
            return null;
        }

        public void Process(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void Process(ICommunicationServer cs, int id)
        {
            this.PlayerId = id;
            //find and set gameId 
            cs.Send(this, cs.GetGameId(GameName));
        }

        public string ToLog()
        {
            throw new NotImplementedException();
        }

        public string PlayerGuid { get; set; }
        public ActionInfo GetActionInfo()
        {
            //create new ActionInfo for join game message
            throw new NotImplementedException();
        }
    }
}
