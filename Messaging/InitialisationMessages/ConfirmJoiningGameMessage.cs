using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    public class ConfirmJoiningGameMessage : IResponse
    {
        public ConfirmJoiningGameMessage(int gameId, int playerId, string privateGuid, PlayerBase playerDefiniton)
        {
            this.GameId = gameId;
            this.PlayerId = playerId;
            this.PrivateGuid = privateGuid;
            this.PlayerDefinition = playerDefiniton;
        }

        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public string PrivateGuid { get; set; }
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
            cs.UpdateTeamCount(id, this.PlayerDefinition.Team);
            cs.Send(this, this.PlayerId);
        }
    }
}
