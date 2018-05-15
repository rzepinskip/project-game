using System;
using Common;
using Common.Interfaces;

namespace GameMaster.ActionHandlers
{
    public class AuthorizeKnowledgeExchangeHandler : ActionHandler
    {
        public AuthorizeKnowledgeExchangeHandler(int playerId, int withPlayerId, IKnowledgeExchangeManager knowledgeExchangeManager) : base(playerId, null)
        {
            knowledgeExchangeManager.AssignSubjectToInitiator(playerId, withPlayerId);
        }

        protected override bool Validate()
        {
            return true;
        }

        public override BoardData Respond()
        {
            return null;
        }
    }
}
