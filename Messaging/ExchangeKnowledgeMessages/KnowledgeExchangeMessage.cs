using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;

namespace Messaging.ExchangeKnowledgeMessages
{
    public abstract class KnowledgeExchangeMessage : Message
    {
        public int PlayerId { get; set; }
        public int SenderPlayerId { get; set; }

        public override string ToLog()
        {
            return $" between players {SenderPlayerId} (sender) and {PlayerId} (receiver)";
        }
    }
}