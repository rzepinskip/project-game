using System;
using Common;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    public class ConfirmJoiningGameMessage : IResponse
    {
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

        public void Process(IPlayer player)
        {
            //handle join message
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
