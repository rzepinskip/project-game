using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.ExchangeKnowledgeMessages
{
    [XmlType(XmlRootName)]
    public class RejectKnowledgeExchangeMessage : KnowledgeExchangeMessage
    {
        public const string XmlRootName = "RejectKnowledgeExchange";

        public bool Permanent { get; set; }
    }
}