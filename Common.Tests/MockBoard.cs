using Common.BoardObjects;
using Common.Interfaces;

namespace Common.Tests
{
    public class MockBoard : BoardBase, IGameMasterBoard
    {
        public MockBoard(int boardWidth, int taskAreaSize, int goalAreaSize) : base(boardWidth, taskAreaSize,
            goalAreaSize)
        {
        }

        public void MarkGoalAsCompleted(GoalField goal)
        {
        }

        public void PlacePieceInTaskArea(int pieceId, Location pieceLocation)
        {
            if (IsLocationInTaskArea(pieceLocation))
                (this[pieceLocation] as TaskField).PieceId = pieceId;
        }
    }
}