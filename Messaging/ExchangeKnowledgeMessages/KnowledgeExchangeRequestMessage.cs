using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;

namespace Messaging.ExchangeKnowledgeMessages
{
    public class KnowledgeExchangeRequestMessage : KnowledgeExchangeMessage
    {
        public override IMessage Process(IGameMaster gameMaster)
        {
            throw new NotImplementedException();
        }

        public override void Process(IPlayer player)
        {
            player.HandleExchangeKnowledge(SenderPlayerId);
        }

        public override void Process(ICommunicationServer cs, int id)
        {
            cs.Send(this, PlayerId);
        }
    }
}