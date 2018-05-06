using System;
using System.Collections.Generic;
using System.Text;
using Common.ActionInfo;
using Common.Interfaces;

namespace Messaging.Requests
{
    public class AuthorizeKnowledgeExchangeRequest : Request
    {
        public int WithPlayerId { get; set; }
        public AuthorizeKnowledgeExchangeRequest(Guid playerGuid, int gameId) : base(playerGuid, gameId)
        {
        }

        public override ActionInfo GetActionInfo()
        {
            return new ExchangeKnowledgeInfo(PlayerGuid, WithPlayerId);
        }
    }
}