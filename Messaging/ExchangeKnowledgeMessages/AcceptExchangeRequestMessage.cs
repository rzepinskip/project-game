using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Messaging.ExchangeKnowledgeMessages
{
    [XmlType(XmlRootName)]
    public class AcceptExchangeRequestMessage : KnowledgeExchangeMessage
    {
        public const string XmlRootName = "AcceptExchangeRequest";
    }
}
