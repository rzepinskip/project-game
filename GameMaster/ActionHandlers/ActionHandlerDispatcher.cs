using Common;
using Common.ActionInfo;

namespace GameMaster.ActionHandlers
{
    internal class ActionHandlerDispatcher
    {
        private readonly ActionHandler _actionHandler;

        public ActionHandlerDispatcher(dynamic actionInfo, GameMasterBoard board, int playerId)
        {
            _actionHandler = ActionHandler(actionInfo, board, playerId);
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

        public DataFieldSet Execute()
        {
            return _actionHandler.Respond();
        }
    }
}