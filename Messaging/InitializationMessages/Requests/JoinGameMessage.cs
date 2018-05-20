using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Common;
using Common.Interfaces;

namespace Messaging.InitializationMessages
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
            try
            {
                var gameId = cs.GetGameIdFor(GameName);
                cs.Send(this, gameId);
            }
            catch (Exception e)
            {
                if (e is InvalidOperationException)
                {
                    Console.WriteLine($"{PlayerId} tried to join non-existent game");
                    cs.Send(new RejectJoiningGame(GameName, PlayerId), id);
                    return;
                }

                throw;
            }
        }

        public override string ToLog()
        {
            return string.Join(',', PlayerId, XmlRootName);
        }
    }
}