using System;
using System.Linq;
using Shared;
using Shared.BoardObjects;

namespace GameMaster
{
    public class PieceGenerator
    {
        private readonly Board _board;
        private readonly Random _random = new Random();
        private readonly double _shamProbability;


        public PieceGenerator(Board board, double shamProbability)
        {
            _board = board;
            _shamProbability = shamProbability;
        }

        public void SpawnPiece()
        {
            var location = GetLocationWithoutPiece();
            var taskField = _board.Content[location.X, location.Y] as TaskField;

            var pieceId = _board.Pieces.Count > 0 ? _board.Pieces.Keys.ToList().Max() + 1 : 0;
            var type = _random.NextDouble() <= _shamProbability
                ? CommonResources.PieceType.Sham
                : CommonResources.PieceType.Normal;

            var piece = new Piece(pieceId, type);
            _board.Pieces.Add(pieceId, piece);
            taskField.PieceId = pieceId;
        }

        private Location GetLocationWithoutPiece()
        {
            var location = new Location();

            var taskAreaBottomLeftCorner = new Location(0, _board.GoalAreaSize);
            var taskAreaTopRightCorner = new Location(_board.Width - 1, _board.Height - (_board.GoalAreaSize + 1));
            do
            {
                var randomX = _random.Next(taskAreaBottomLeftCorner.X, taskAreaTopRightCorner.X + 1);
                var randomY = _random.Next(taskAreaBottomLeftCorner.Y, taskAreaTopRightCorner.Y + 1);
                location = new Location(randomX, randomY);
            } while (_board.GetPieceFromBoard(location) != null);

            return location;
        }
    }
}