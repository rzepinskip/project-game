using Shared;
using Shared.ActionAvailability.ActionAvailabilityHelpers;
using Shared.BoardObjects;
using Shared.GameMessages;

namespace Player.Strategy.StateTransition
{
    internal class DiscoverTransition : BaseTransition
    {
        public DiscoverTransition(Location location, CommonResources.TeamColour team, int playerId, Board board) : base(
            location, team, playerId, board)
        {
        }

        public override GameMessage ExecuteStrategy()
        {
            var directionToNearest = CommonResources.MoveType.Left;
            var distanceToNearest = int.MaxValue;


            //New chain of resposibility, maybe (?)
            if (new MoveAvailability().IsInsideBoard(location, CommonResources.MoveType.Left, board.Width, board.Height)
            )
                CheckIfCloser(board, new Location(location.X - 1, location.Y), ref distanceToNearest,
                    CommonResources.MoveType.Left, ref directionToNearest);
            if (new MoveAvailability().IsInsideBoard(location, CommonResources.MoveType.Right, board.Width,
                board.Height))
                CheckIfCloser(board, new Location(location.X + 1, location.Y), ref distanceToNearest,
                    CommonResources.MoveType.Right, ref directionToNearest);
            if (new MoveAvailability().IsInsideBoard(location, CommonResources.MoveType.Down, board.Width, board.Height)
            )
                CheckIfCloser(board, new Location(location.X, location.Y - 1), ref distanceToNearest,
                    CommonResources.MoveType.Down, ref directionToNearest);
            if (new MoveAvailability().IsInsideBoard(location, CommonResources.MoveType.Up, board.Width, board.Height))
                CheckIfCloser(board, new Location(location.X, location.Y + 1), ref distanceToNearest,
                    CommonResources.MoveType.Up, ref directionToNearest);


            ChangeState = PlayerStrategy.PlayerState.MoveToPiece;
            return new Move
            {
                Direction = directionToNearest,
                PlayerId = playerId
            };
        }

        private void CheckIfCloser(Board board, Location newLocation, ref int distanceToNearest,
            CommonResources.MoveType direction, ref CommonResources.MoveType directionToNearest)
        {
            var taskField = board.Content[newLocation.X, newLocation.Y] as TaskField;
            if (taskField != null)
                if (taskField.DistanceToPiece < distanceToNearest)
                {
                    distanceToNearest = taskField.DistanceToPiece;
                    directionToNearest = direction;
                }
        }
    }
}