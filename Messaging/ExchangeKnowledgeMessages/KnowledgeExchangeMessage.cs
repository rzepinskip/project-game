using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;

namespace Messaging.ExchangeKnowledgeMessages
{
    public abstract class KnowledgeExchangeMessage : IMessage
    {
        public int PlayerId { get; set; }
        public int SenderPlayerId { get; set; }
        public IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public void Process(IPlayer player)
        {
            //TODO
        }
    }
}