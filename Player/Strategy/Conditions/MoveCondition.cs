using Player.Strategy.States;
using Shared;
using Shared.ActionAvailability.ActionAvailabilityHelpers;
using Shared.BoardObjects;
using Shared.GameMessages;

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

        public override GameMessage GetNextMessage(State fromState)
        {
            var directionToNearest = CommonResources.MoveType.Left;
            var distanceToNearest = int.MaxValue;
            var currentLocation = StrategyInfo.FromLocation;
            var boardWidth = StrategyInfo.Board.Width;
            var boardHeight = StrategyInfo.Board.Height;
            var board = StrategyInfo.Board;
            //New chain of resposibility, maybe (?)
            if (new MoveAvailability().IsInsideBoard(currentLocation, CommonResources.MoveType.Left, boardWidth,
                boardHeight))
                CheckIfCloser(board, new Location(currentLocation.X - 1, currentLocation.Y), ref distanceToNearest,
                    CommonResources.MoveType.Left, ref directionToNearest);
            if (new MoveAvailability().IsInsideBoard(currentLocation, CommonResources.MoveType.Right, boardWidth,
                boardHeight))
                CheckIfCloser(board, new Location(currentLocation.X + 1, currentLocation.Y), ref distanceToNearest,
                    CommonResources.MoveType.Right, ref directionToNearest);
            if (new MoveAvailability().IsInsideBoard(currentLocation, CommonResources.MoveType.Down, boardWidth,
                boardHeight))
                CheckIfCloser(board, new Location(currentLocation.X, currentLocation.Y - 1), ref distanceToNearest,
                    CommonResources.MoveType.Down, ref directionToNearest);
            if (new MoveAvailability().IsInsideBoard(currentLocation, CommonResources.MoveType.Up, boardWidth,
                boardHeight))
                CheckIfCloser(board, new Location(currentLocation.X, currentLocation.Y + 1), ref distanceToNearest,
                    CommonResources.MoveType.Up, ref directionToNearest);

            return new Move
            {
                Direction = directionToNearest,
                PlayerId = StrategyInfo.PlayerId
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