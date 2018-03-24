using System;
using System.Linq;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace GameMaster
{
    public class PieceGenerator
    {
        private readonly GameMasterBoard _board;
        private readonly Random _random = new Random();
        private readonly double _shamProbability;

        public PieceGenerator(GameMasterBoard board, double shamProbability)
        {
            _board = board;
            _shamProbability = shamProbability;
        }

        public void SpawnPiece()
        {
            lock (_board.Lock)
            {
                var location = GetLocationWithoutPiece();
            var taskField = _board[location] as TaskField;

                var pieceId = _board.Pieces.Count > 0 ? _board.Pieces.Keys.ToList().Max() + 1 : 0;
                var type = _random.NextDouble() <= _shamProbability
                ? PieceType.Sham
                : PieceType.Normal;

                var piece = new Piece(pieceId, type);
                _board.Pieces.Add(pieceId, piece);
                taskField.PieceId = pieceId;
            }
        }

        private Location GetLocationWithoutPiece()
        {
            Location location;

            var taskAreaBottomLeftCorner = new Location(0, _board.GoalAreaSize);
            var taskAreaTopRightCorner = new Location(_board.Width - 1, _board.Height - (_board.GoalAreaSize + 1));
            do
            {
                var randomX = _random.Next(taskAreaBottomLeftCorner.X, taskAreaTopRightCorner.X + 1);
                var randomY = _random.Next(taskAreaBottomLeftCorner.Y, taskAreaTopRightCorner.Y + 1);
                location = new Location(randomX, randomY);
            } while (_board.GetPieceIdAt(location) != null);

            return location;
        }
    }
}