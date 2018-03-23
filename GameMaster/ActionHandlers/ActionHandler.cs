using Common;

namespace GameMaster.ActionHandlers
{
    public abstract class ActionHandler
    {
        protected GameMasterBoard Board;
        protected int PlayerId;

        protected ActionHandler(int playerId, GameMasterBoard board)
        {
            Board = board;
            PlayerId = playerId;
        }

        protected abstract bool Validate();
        public abstract DataFieldSet Respond();
    }
}