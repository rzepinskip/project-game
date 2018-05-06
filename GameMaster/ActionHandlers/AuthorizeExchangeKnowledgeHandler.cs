using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace GameMaster.ActionHandlers
{
    public class AuthorizeExchangeKnowledgeHandler : ActionHandler
    {
        public AuthorizeExchangeKnowledgeHandler(int playerId, GameMasterBoard board) : base(playerId, board)
        {
        }

        protected override bool Validate()
        {
            throw new NotImplementedException();
        }

        public override DataFieldSet Respond()
        {
            throw new NotImplementedException();
        }
    }
}
