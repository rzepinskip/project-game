using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Board
{
    public class Board
    {
        public Field[,] Content { get; }
        public int TaskAreaSize { get; }
        public int GoalAreaSize { get; }
        public int Width { get; }
        public Board(int boardWidth, int taskAreaSize, int goalAreaSize) {
            this.GoalAreaSize = goalAreaSize;
            this.TaskAreaSize = taskAreaSize;
            this.Width = boardWidth;
            this.Content = new Field[boardWidth, taskAreaSize + 2 * goalAreaSize];

            for (int i = 0; i < boardWidth; ++i) {
                for (int j = 0; j < goalAreaSize; ++j)
                    this.Content[i, j] = new GoalField();
                for (int j = goalAreaSize; j < taskAreaSize + goalAreaSize; ++j)
                    this.Content[i, j] = new TaskField();
                for (int j = taskAreaSize + goalAreaSize; j < 2 * goalAreaSize + taskAreaSize; ++j)
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
