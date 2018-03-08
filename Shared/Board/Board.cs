using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Board
{
    public class Board
    {
        public Location[,] Content { get; }
        public int TaskAreaSize { get; }
        public int GoalAreaSize { get; }
        public int Width { get; }
        public Board(int boardWidth, int taskAreaSize, int goalAreaSize) {
            this.GoalAreaSize = goalAreaSize;
            this.TaskAreaSize = taskAreaSize;
            this.Width = boardWidth;
            this.Content = new Location[taskAreaSize + 2 * goalAreaSize, boardWidth];
        }
    }
}
