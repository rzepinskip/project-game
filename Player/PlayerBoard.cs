using Common.BoardObjects;
using Common.Interfaces;

namespace Player
{
    public class PlayerBoard : BoardBase, IBoard
    {
        public PlayerBoard(int boardWidth, int taskAreaSize, int goalAreaSize) : base(boardWidth, taskAreaSize,
            goalAreaSize)
        {
        }
    }
}