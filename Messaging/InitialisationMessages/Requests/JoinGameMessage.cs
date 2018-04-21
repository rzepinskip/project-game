using System;
using System.Xml.Serialization;
using Common;
using Common.ActionInfo;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    /// <summary>
    /// Player's request to GM to join game
    /// </summary>
    [XmlType(XmlRootName)]
    public class JoinGameMessage : Message
    {
        public const string XmlRootName = "JoinGame";

        public int PlayerId;

        public JoinGameMessage()
        {
        }

        public JoinGameMessage(string gameName, PlayerType preferedRole, TeamColor preferedTeam)
        {
            GameName = gameName;
            PreferedRole = preferedRole;
            PreferedTeam = preferedTeam;
        }

        public string GameName { get; set; }
        public PlayerType PreferedRole { get; set; }
        public TeamColor PreferedTeam { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            if(!gameMaster.IsSlotAvailable())
                return new RejectJoiningGame();

            gameMaster.AssignPlayerToAvailableSlotWithPrefered(PreferedTeam, PreferedRole);

            var playerTeam = PreferedTeam;
            var playerType = PreferedRole;

            if (!gameMaster.IsSlotAvailableIn(PreferedTeam))
            {
                playerTeam = PreferedTeam == TeamColor.Blue ? TeamColor.Red : TeamColor.Blue;
            }
            else
            {
                //return new RejectJoiningGame(GameName, PlayerId);
            }

            if (PreferedRole == PlayerType.Leader)
                if (gameMaster.DoesTeamHasAlreadyLeader(PreferedTeam))
                    playerType = PlayerType.Member;


            gameMaster.AssignPlayerToTeam(PlayerId, playerTeam, playerType);

            //return new ConfirmJoiningGameMessage();
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
    }
}