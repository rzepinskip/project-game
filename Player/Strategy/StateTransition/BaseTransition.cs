using Shared.BoardObjects;
using Shared.GameMessages;
using static Player.Strategy.PlayerStrategy;
using static Shared.CommonResources;

namespace Player.Strategy.StateTransition
{
    public abstract class BaseTransition
    {
        protected Board board;
        protected Location location;
        protected int playerId;
        protected TeamColour team;

        public BaseTransition(Location location, TeamColour team, int playerId, Board board)
        {
            this.location = location;
            this.team = team;
            this.playerId = playerId;
            this.board = board;
        }

        public PlayerState ChangeState { get; set; }

        public abstract GameMessage ExecuteStrategy();
    }
}