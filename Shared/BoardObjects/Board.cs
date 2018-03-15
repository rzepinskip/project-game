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



        public Board(int boardWidth, int taskAreaSize, int goalAreaSize)
        {
            this.GoalAreaSize = goalAreaSize;
            this.TaskAreaSize = taskAreaSize;
            this.Width = boardWidth;
            this.Content = new Field[boardWidth, Height];
            this.Players = new Dictionary<int, PlayerInfo>();
            this.Pieces = new Dictionary<int, Piece>();

            for (var i = 0; i < boardWidth; ++i)
            {
                for (var j = 0; j < goalAreaSize; ++j)
                    this.Content[i, j] = new GoalField() { X = i, Y = j };
                for (var j = goalAreaSize; j < taskAreaSize + goalAreaSize; ++j)
                    this.Content[i, j] = new TaskField() { X = i, Y = j };
                for (var j = taskAreaSize + goalAreaSize; j < this.Height; ++j)
                    this.Content[i, j] = new GoalField() { X = i, Y = j };
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
        public int GetDistanceToPiece(Location l)
        {
            var min = int.MaxValue;
            for (var i = 0; i < Width; ++i)
                for (var j = GoalAreaSize; j < TaskAreaSize + GoalAreaSize; ++j)
                {
                    var field = Content[i, j] as TaskField;
                    if (field.PieceId != null)
                    {
                        var distance = field.GetManhattanDistance(l);
                        if (distance < min)
                            min = distance;
                    }
                }
            if (min == int.MaxValue)
                min = -1;

            return min;
        }
    }
}
