using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.BoardObjects
{
    public class Board
    {
        public Field[,] Content { get; }
        public int TaskAreaSize { get; }
        public int GoalAreaSize { get; }
        public int Width { get; }
        public int Height
        {
            get
            {
                return 2 * this.GoalAreaSize + TaskAreaSize;
            }
        }

        public Dictionary<int, PlayerInfo> Players { get; }
        public Dictionary<int, Piece> Pieces { get; }


        public Board(int boardWidth, int taskAreaSize, int goalAreaSize) {
            this.GoalAreaSize = goalAreaSize;
            this.TaskAreaSize = taskAreaSize;
            this.Width = boardWidth;
            this.Content = new Field[boardWidth, Height];
            this.Players = new Dictionary<int, PlayerInfo>();
            this.Pieces = new Dictionary<int, Piece>();

            for (var i = 0; i < boardWidth; ++i)
            {
                for (var j = 0; j < goalAreaSize; ++j)
                    this.Content[i, j] = new GoalField();
                for (var j = goalAreaSize; j < taskAreaSize + goalAreaSize; ++j)
                    this.Content[i, j] = new TaskField();
                for (var j = taskAreaSize + goalAreaSize; j < this.Height; ++j)
                    this.Content[i, j] = new GoalField();
            }
        }
        public bool IsLocationInTaskArea(Location l)
        {
            if (l.Y <= TaskAreaSize + GoalAreaSize - 1 && l.Y >= GoalAreaSize)
                return true;
            return false;

        }
        public void PlacePieceInTaskArea(int PieceId, Location l)
        {
            if (IsLocationInTaskArea(l))
                ((TaskField)Content[l.X, l.Y]).PieceId = PieceId;
        }
        public int? GetPieceFromBoard(Location l)
        {
            int? pieceId = null;
            if (IsLocationInTaskArea(l))
            {
                pieceId = ((TaskField)Content[l.X, l.Y]).PieceId;
            }
            return pieceId;

        }
    }
}
