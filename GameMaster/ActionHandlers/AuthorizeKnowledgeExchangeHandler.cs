using System;
using Common;
using Common.Interfaces;

namespace GameMaster.ActionHandlers
{
    public class AuthorizeKnowledgeExchangeHandler : ActionHandler
    {
        public AuthorizeKnowledgeExchangeHandler(int playerId, GameMasterBoard board, int withPlayerId,  IKnowledgeExchangeManager knowledgeExchangeManager) : base(playerId, board)
        {
            Console.WriteLine($"Authorize forward from {playerId}({Board.Players[playerId].Team}) to {withPlayerId}({Board.Players[withPlayerId].Team})");
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
