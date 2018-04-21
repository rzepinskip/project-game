using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.BoardObjects;

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

                if (location == null)
                    return;

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
            var taskAreaBottomLeftCorner = new Location(0, _board.GoalAreaSize);
            var taskAreaTopRightCorner = new Location(_board.Width - 1, _board.Height - (_board.GoalAreaSize + 1));

            var locations = new List<Location>();
            for (var x = taskAreaBottomLeftCorner.X; x < taskAreaTopRightCorner.X + 1; x++)
            for (var y = taskAreaBottomLeftCorner.Y; y < taskAreaTopRightCorner.Y + 1; y++)
            {
                var location = new Location(x, y);

                if ((_board[location] as TaskField).PieceId == null)
                    locations.Add(location);
            }

            if (locations.Count == 0)
                return null;

            var randomIndex = _random.Next(0, locations.Count);

            return locations[randomIndex];
        }
    }
}