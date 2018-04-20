using System;
using System.Xml.Serialization;
using Common;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    [XmlType(XmlRootName)]
    public class ConfirmJoiningGameMessage : IResponse
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
        public IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public bool Process(IPlayer player)
        {
            //handle join message

            return false;
        }

        public void Process(ICommunicationServer cs, int id)
        {
            //update team count
            cs.UpdateTeamCount(id, PlayerDefinition.Team);
            cs.AssignGameIdToPlayerId(id, PlayerId);
            cs.Send(this, PlayerId);
        }
    }
}
