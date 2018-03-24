using Common;
using Common.ActionAvailability.Helpers;
using Common.BoardObjects;
using Messaging.Requests;

namespace Player.Strategy.StateTransition
{
    internal class DiscoverTransition : BaseTransition
    {
        public DiscoverTransition(Location location, TeamColor team, int playerId, PlayerBoard board) : base(
            location, team, playerId, board)
        {
        }

        public override Request ExecuteStrategy()
        {
            var directionToNearest = Direction.Left;
            var distanceToNearest = int.MaxValue;


            //New chain of resposibility, maybe (?)
            if (new MoveAvailability().IsInsideBoard(location, Direction.Left, board.Width, board.Height)
            )
                CheckIfCloser(board, new Location(location.X - 1, location.Y), ref distanceToNearest,
                    Direction.Left, ref directionToNearest);
            if (new MoveAvailability().IsInsideBoard(location, Direction.Right, board.Width,
                board.Height))
                CheckIfCloser(board, new Location(location.X + 1, location.Y), ref distanceToNearest,
                    Direction.Right, ref directionToNearest);
            if (new MoveAvailability().IsInsideBoard(location, Direction.Down, board.Width, board.Height)
            )
                CheckIfCloser(board, new Location(location.X, location.Y - 1), ref distanceToNearest,
                    Direction.Down, ref directionToNearest);
            if (new MoveAvailability().IsInsideBoard(location, Direction.Up, board.Width, board.Height))
                CheckIfCloser(board, new Location(location.X, location.Y + 1), ref distanceToNearest,
                    Direction.Up, ref directionToNearest);


            ChangeState = PlayerState.MoveToPiece;
            return new MoveRequest(playerId, directionToNearest);
        }

        private void CheckIfCloser(PlayerBoard board, Location newLocation, ref int distanceToNearest,
            Direction direction, ref Direction directionToNearest)
        {
            var taskField = board[newLocation] as TaskField;
            if (taskField != null)
                if (taskField.DistanceToPiece < distanceToNearest)
                {
                    distanceToNearest = taskField.DistanceToPiece;
                    directionToNearest = direction;
                }
        }
    }
}