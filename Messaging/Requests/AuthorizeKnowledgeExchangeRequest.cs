using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Common;
using Common.ActionInfo;
using Common.Interfaces;

namespace Messaging.Requests
{
    [XmlType(XmlRootName)]
    public class AuthorizeKnowledgeExchangeRequest : Request
    {
        public const string XmlRootName = "AuthorizeKnowledgeExchange";

        public int WithPlayerId { get; set; }

        public AuthorizeKnowledgeExchangeRequest()
        {
        }

        public AuthorizeKnowledgeExchangeRequest(Guid playerGuid, int gameId) : base(playerGuid, gameId)
        {
        }

        public override ActionInfo GetActionInfo()
        {
            return new ExchangeKnowledgeInfo(PlayerGuid, WithPlayerId);
        }

        public override string ToLog()
        {
            return string.Join(',', ActionType.AuthorizeKnowledgeExchange, base.ToLog());
        }
    }
}