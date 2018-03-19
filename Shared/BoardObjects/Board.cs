using System;
using System.Collections.Generic;
using static Shared.CommonResources;

namespace Shared.BoardObjects
{
    public class Board
    {
        public Board(int boardWidth, int taskAreaSize, int goalAreaSize)
        {
            GoalAreaSize = goalAreaSize;
            TaskAreaSize = taskAreaSize;
            Width = boardWidth;
            Content = new Field[boardWidth, Height];
            Players = new Dictionary<int, PlayerInfo>();
            Pieces = new Dictionary<int, Piece>();

            for (var i = 0; i < boardWidth; ++i)
            {
                for (var j = 0; j < goalAreaSize; ++j)
                    Content[i, j] = new GoalField(GoalFieldType.Unknown, TeamColour.Blue, null, DateTime.Now, i, j);
                for (var j = goalAreaSize; j < taskAreaSize + goalAreaSize; ++j)
                    Content[i, j] = new TaskField(-1, null, null, DateTime.Now, i, j);
                for (var j = taskAreaSize + goalAreaSize; j < Height; ++j)
                    Content[i, j] = new GoalField(GoalFieldType.Unknown, TeamColour.Red, null, DateTime.Now, i, j);
            }
        }

        public Field[,] Content { get; }
        public int TaskAreaSize { get; }
        public int GoalAreaSize { get; }
        public int Width { get; }

        public int Height => 2 * GoalAreaSize + TaskAreaSize;

        public Dictionary<int, PlayerInfo> Players { get; }
        public Dictionary<int, Piece> Pieces { get; }

        public bool IsLocationInTaskArea(Location l)
        {
            if (l.Y <= TaskAreaSize + GoalAreaSize - 1 && l.Y >= GoalAreaSize)
                return true;
            return false;
        }

        public void PlacePieceInTaskArea(int PieceId, Location l)
        {
            if (IsLocationInTaskArea(l))
                ((TaskField) Content[l.X, l.Y]).PieceId = PieceId;
        }

        public int? GetPieceFromBoard(Location l)
        {
            int? pieceId = null;
            if (IsLocationInTaskArea(l)) pieceId = ((TaskField) Content[l.X, l.Y]).PieceId;
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