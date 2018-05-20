using System;
using Common;
using Common.ActionInfo;
using Common.Interfaces;

namespace GameMaster.ActionHandlers
{
    internal class ActionHandlerDispatcher
    {
        private readonly GameMasterBoard _board;
        private readonly IKnowledgeExchangeManager _knowledgeExchangeManager;

        public ActionHandlerDispatcher(GameMasterBoard board, IKnowledgeExchangeManager knowledgeExchangeManager)
        {
            _board = board;
            _knowledgeExchangeManager = knowledgeExchangeManager;
        }

        public ActionHandler Resolve(dynamic actionInfo, int playerId)
        {
            return ActionHandler(actionInfo, _board, playerId);
        }

        public MoveActionHandler ActionHandler(MoveActionInfo actionInfo, GameMasterBoard board, int playerId)
        {
            return new MoveActionHandler(playerId, board, actionInfo.Direction);
        }

        public DiscoverActionHandler ActionHandler(DiscoverActionInfo actionInfo, GameMasterBoard board, int playerId)
        {
            return new DiscoverActionHandler(playerId, board);
        }

        public PickUpActionHandler ActionHandler(PickUpActionInfo actionInfo, GameMasterBoard board, int playerId)
        {
            return new PickUpActionHandler(playerId, board);
        }

        public PlacePieceActionHandler ActionHandler(PlaceActionInfo actionInfo, GameMasterBoard board, int playerId)
        {
            return new PlacePieceActionHandler(playerId, board);
        }

        public TestPieceActionHandler ActionHandler(TestActionInfo actionInfo, GameMasterBoard board, int playerId)
        {
            return new TestPieceActionHandler(playerId, board);
        }

        public DestroyPieceActionHandler ActionHandler(DestroyActionInfo actionInfo, GameMasterBoard board,
            int playerId)
        {
            return new DestroyPieceActionHandler(playerId, board);
        }

        public AuthorizeKnowledgeExchangeHandler ActionHandler(KnowledgeExchangeInfo actionInfo, GameMasterBoard board,
            int playerId)
        {
            return new AuthorizeKnowledgeExchangeHandler(playerId, board, actionInfo.SubjectId, _knowledgeExchangeManager);
        }
    }
}