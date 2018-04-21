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
            var playerTeam = PreferedTeam;
            var playerType = PreferedRole;

            if (!gameMaster.IsPlaceOnTeam(PreferedTeam))
            {
                playerTeam = PreferedTeam == TeamColor.Blue ? TeamColor.Red : TeamColor.Blue;
            } 
            else
            {
                return new RejectJoiningGame(GameName, PlayerId);
            }

            if (PreferedRole == PlayerType.Leader)
            {
                if (!gameMaster.IsLeaderInTeam(PreferedTeam))
                    playerType = PlayerType.Member;
            }

            

            return gameMaster.AssignPlayerToTeam(PlayerId, playerTeam, playerType);
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
