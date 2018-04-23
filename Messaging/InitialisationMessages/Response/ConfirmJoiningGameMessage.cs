using System;
using System.Xml.Serialization;
using Common;
using Common.Interfaces;
using Messaging.Responses;

namespace Messaging.InitialisationMessages
{
    /// <summary>
    ///     GM sends confirmation to player about his registration
    /// </summary>
    [XmlType(XmlRootName)]
    public class ConfirmJoiningGameMessage : Response
    {
        public const string XmlRootName = "ConfirmJoiningGame";

        protected ConfirmJoiningGameMessage()
        {
        }

        public ConfirmJoiningGameMessage(int gameId, int playerId, Guid privateGuid, PlayerBase playerDefiniton)
        {
            GameId = gameId;
            PlayerId = playerId;
            PrivateGuid = privateGuid;
            PlayerDefinition = playerDefiniton;
        }

        [XmlAttribute("gameId")] public int GameId { get; set; }
        [XmlAttribute("privateGuid")] public Guid PrivateGuid { get; set; }
        public PlayerBase PlayerDefinition { get; set; }

        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override void Process(IPlayer player)
        {
            player.UpdateJoiningInfo(true);
            player.UpdatePlayer(PlayerId, PrivateGuid, PlayerDefinition, GameId);
        }

        public override void Process(ICommunicationServerCommunicator cs, int id)
        {
            //update team count
            cs.UpdateTeamCount(id, PlayerDefinition.Team);
            cs.AssignGameIdToPlayerId(id, PlayerId);
            cs.Send(this, PlayerId);
        }
    }
}