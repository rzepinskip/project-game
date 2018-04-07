using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;

namespace Messaging.InitialisationMessages
{
    public class RejectJoiningGame : IMessage
    {
        public RejectJoiningGame(string gameName, int playerId)
        {
            this.GameName = gameName;
            this.PlayerId = playerId;
        }
        public string GameName { get; set; }
        public int PlayerId { get; set; }
        public IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public void Process(IPlayer player)
        {
            throw new NotImplementedException();
        }
    }
}
