using Common.BoardObjects;
using Common.Interfaces;

namespace GameMaster
{
    public class GameMasterBoard : BoardBase, IBoard
    {
        public GameMasterBoard(int boardWidth, int taskAreaSize, int goalAreaSize) : base(boardWidth, taskAreaSize, goalAreaSize)
        {
        }
    }
}