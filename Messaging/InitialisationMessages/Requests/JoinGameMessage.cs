using System;
using System.Xml.Serialization;
using Common;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    /// <summary>
    ///     Player's request to GM to join game
    /// </summary>
    [XmlType(XmlRootName)]
    public class JoinGameMessage : Message
    {
        public const string XmlRootName = "JoinGame";

        public int PlayerId;

        protected JoinGameMessage()
        {
        }

        public JoinGameMessage(string gameName, PlayerType preferedRole, TeamColor preferedTeam)
        {
            GameName = gameName;
            PreferedRole = preferedRole;
            PreferedTeam = preferedTeam;
        }

        [XmlAttribute("gameName")] public string GameName { get; set; }
        [XmlAttribute("preferredRole")] public PlayerType PreferedRole { get; set; }
        [XmlAttribute("preferredTeam")] public TeamColor PreferedTeam { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            if (!gameMaster.IsSlotAvailable())
                return new RejectJoiningGame(GameName, PlayerId);

            var (gameId, guid, playerInfo) =
                gameMaster.AssignPlayerToAvailableSlotWithPrefered(PlayerId, PreferedTeam, PreferedRole);

            return new ConfirmJoiningGameMessage(gameId, PlayerId, guid, playerInfo);
        }

        public override void Process(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            PlayerId = id;
            cs.MarkClientAsPlayer(id);
            cs.Send(this, cs.GetGameId(GameName));
        }
    }
}