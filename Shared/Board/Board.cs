using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Board
{
    class Board
    {
        public BoardObject[,] Content { get; }
        public int TaskAreaSize { get; }
        public int GoalAreaSize { get; }

        public Board(int boardWidth, int taskAreaSize, int goalAreaSize) {
            this.GoalAreaSize = goalAreaSize;
            this.TaskAreaSize = taskAreaSize;
            this.Content = new BoardObject[taskAreaSize + 2 * goalAreaSize, boardWidth];
        }
    }
}
