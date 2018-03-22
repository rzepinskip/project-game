using Common;
using Common.BoardObjects;
using Messaging.Requests;

namespace Player.Strategy.StateTransition
{
    public abstract class BaseTransition
    {
        protected PlayerBoard board;
        protected Location location;
        protected int playerId;
        protected TeamColor team;

        public BaseTransition(Location location, TeamColor team, int playerId, PlayerBoard board)
        {
            this.location = location;
            this.team = team;
            this.playerId = playerId;
            this.board = board;
        }

        public PlayerState ChangeState { get; set; }

        public abstract Request ExecuteStrategy();
    }
}