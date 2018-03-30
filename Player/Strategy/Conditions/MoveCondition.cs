using Common;
using Common.ActionAvailability.Helpers;
using Common.BoardObjects;
using Messaging.Requests;
using Player.Strategy.States;

namespace Player.Strategy.Conditions
{
    public class MoveCondition : Condition
    {
        public MoveCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            return true;
        }

        public override State GetNextState(State fromState)
        {
            return new MoveToPieceState(StrategyInfo);
        }

        public override Request GetNextMessage(State fromState)
        {
            var directionToNearest = Direction.Left;
            var distanceToNearest = int.MaxValue;
            var currentLocation = StrategyInfo.FromLocation;
            var boardWidth = StrategyInfo.Board.Width;
            var boardHeight = StrategyInfo.Board.Height;
            var board = StrategyInfo.Board;
            //New chain of resposibility, maybe (?)
            if (new MoveAvailability().IsInsideBoard(currentLocation, Direction.Left, boardWidth,
                boardHeight))
                CheckIfCloser(board, new Location(currentLocation.X - 1, currentLocation.Y), ref distanceToNearest,
                    Direction.Left, ref directionToNearest);
            if (new MoveAvailability().IsInsideBoard(currentLocation, Direction.Right, boardWidth,
                boardHeight))
                CheckIfCloser(board, new Location(currentLocation.X + 1, currentLocation.Y), ref distanceToNearest,
                    Direction.Right, ref directionToNearest);
            if (new MoveAvailability().IsInsideBoard(currentLocation, Direction.Down, boardWidth,
                boardHeight))
                CheckIfCloser(board, new Location(currentLocation.X, currentLocation.Y - 1), ref distanceToNearest,
                    Direction.Down, ref directionToNearest);
            if (new MoveAvailability().IsInsideBoard(currentLocation, Direction.Up, boardWidth,
                boardHeight))
                CheckIfCloser(board, new Location(currentLocation.X, currentLocation.Y + 1), ref distanceToNearest,
                    Direction.Up, ref directionToNearest);

            StrategyInfo.ToLocation = StrategyInfo.FromLocation.GetNewLocation(directionToNearest);
            return new MoveRequest(StrategyInfo.PlayerGuid, directionToNearest);
        }

        private void CheckIfCloser(BoardBase board, Location newLocation, ref int distanceToNearest,
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