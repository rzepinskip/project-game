using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ActionInfo
{
    public class ExchangeKnowledgeInfo : ActionInfo
    {
        public ExchangeKnowledgeInfo(Guid playerGuid, int withPlayerId) : base(playerGuid)
        {
            WithPlayerId = withPlayerId;
        }

        public int WithPlayerId { get; set; }
    }
}