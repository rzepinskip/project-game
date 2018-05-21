using System.Collections.Generic;
using ClientsCommon.ActionAvailability.Helpers;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Requests;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.Common.Transitions;
using PlayerStateCoordinator.NormalPlayer.States;

namespace PlayerStateCoordinator.NormalPlayer.Transitions
{
    public class MoveToPieceStrategyTransition : NormalPlayerStrategyTransition
    {
        public MoveToPieceStrategyTransition(NormalPlayerStrategyInfo normalPlayerStrategyInfo) : base(
            normalPlayerStrategyInfo)
        {
        }

        public override State NextState => new MoveToPieceStrategyState(NormalPlayerStrategyInfo);

        public override IEnumerable<IMessage> Message
        {
            get
            {
                var directionToNearest = Direction.Left;
                var distanceToNearest = int.MaxValue;
                var currentLocation = NormalPlayerStrategyInfo.CurrentLocation;
                var boardWidth = NormalPlayerStrategyInfo.Board.Width;
                var boardHeight = NormalPlayerStrategyInfo.Board.Height;
                var board = NormalPlayerStrategyInfo.Board;
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

                NormalPlayerStrategyInfo.TargetLocation = currentLocation.GetNewLocation(directionToNearest);

                return new List<IMessage>
                {
                    new MoveRequest(NormalPlayerStrategyInfo.PlayerGuid, NormalPlayerStrategyInfo.GameId,
                        directionToNearest)
                };
            }
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

        public override bool IsPossible()
        {
            return true;
        }
    }
}