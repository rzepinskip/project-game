using Common;
using Common.ActionAvailability.Helpers;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy.Conditions.StrategyConditions
{
    public class MoveStrategyCondition : StrategyCondition
    {
        public MoveStrategyCondition(StrategyInfo strategyInfo) : base(strategyInfo)
        {
        }

        public override bool CheckCondition()
        {
            return true;
        }

        public override BaseState GetNextState(BaseState fromStrategyState)
        {
            return new MoveToPieceStrategyState(StrategyInfo);
        }

        public override IMessage GetNextMessage(BaseState fromStrategyState)
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
            return new MoveRequest(StrategyInfo.PlayerGuid, StrategyInfo.GameId, directionToNearest);
        }

        public override bool ReturnsMessage(BaseState fromStrategyState)
        {
            return true;
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