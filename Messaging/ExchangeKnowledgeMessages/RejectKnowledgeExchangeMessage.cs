using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.ExchangeKnowledgeMessages
{
    public class RejectKnowledgeExchangeMessage : KnowledgeExchangeMessage
    {
        public bool Permanent { get; set; }
    }
}