using System;
using System.Xml.Serialization;
using Common;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    [XmlType(XmlRootName)]
    public class ConfirmJoiningGameMessage : Response
    {
        public const string XmlRootName = "ConfirmJoiningGame";
        
        public ConfirmJoiningGameMessage() { }
        public ConfirmJoiningGameMessage(int gameId, int playerId, Guid privateGuid, PlayerBase playerDefiniton)
        {
            GameId = gameId;
            PlayerId = playerId;
            PrivateGuid = privateGuid;
            PlayerDefinition = playerDefiniton;
        }

        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public Guid PrivateGuid { get; set; }
        public PlayerBase PlayerDefinition { get; set; }
        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override bool Process(IPlayer player)
        {
            //handle join message

            return false;
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            //update team count
            cs.UpdateTeamCount(id, PlayerDefinition.Team);
            cs.AssignGameIdToPlayerId(id, PlayerId);
            cs.Send(this, PlayerId);
        }
    }
}
