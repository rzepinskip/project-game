using Shared;
using Shared.BoardObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaster
{
    public class PieceGenerator
    {
        private Board _board;
        private Random _random = new Random();

        public PieceGenerator(Board board)
        {
            _board = board;
        }

        public void SpawnPiece()
        {
            var shamProbability = 0.1;

            var location = GetLocationWithoutPiece();
            var taskField = _board.Content[location.X, location.Y] as TaskField;

            var pieceId = _board.Pieces.Count > 0 ? _board.Pieces.Keys.ToList().Max() + 1 : 0;
            var type = _random.NextDouble() <= shamProbability ? CommonResources.PieceType.Sham : CommonResources.PieceType.Normal;

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
            }
            while (_board.GetPieceFromBoard(location) != null);

            return location;
        }
    }
}
