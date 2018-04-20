using System;
using System.Xml.Serialization;
using Common;
using Common.ActionInfo;
using Common.Interfaces;
using Messaging.Requests;

namespace Messaging.InitialisationMessages
{
    [XmlType(XmlRootName)]
    public class JoinGameMessage : Request
    {
        public const string XmlRootName = "JoinGame";

        public JoinGameMessage() { }
        public JoinGameMessage(string gameName, PlayerType preferedRole, TeamColor preferedTeam)
        {
            GameName = gameName;
            PreferedRole = preferedRole;
            PreferedTeam = preferedTeam;
        }

        public int PlayerId;
        public string GameName { get; set; }
        public PlayerType PreferedRole { get; set; }
        public TeamColor PreferedTeam { get; set; }
        public override IMessage Process(IGameMaster gameMaster)
        {
            //handle join game message in dispatcher handler
            return null;
        }

        public override bool Process(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            PlayerId = id;
            
            cs.Send(this, cs.GetGameId(GameName));
        }

        public override string ToLog()
        {
            throw new NotImplementedException();
        }

        //public Guid PlayerGuid { get; set; }
        public override ActionInfo GetActionInfo()
        {
            //create new ActionInfo for join game message
            throw new NotImplementedException();
        }
    }
}
