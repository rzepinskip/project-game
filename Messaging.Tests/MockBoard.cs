using Common.BoardObjects;
using Common.Interfaces;

namespace Messaging.Tests
{
    public class MockBoard : BoardBase
    {
        public MockBoard(int boardWidth, int taskAreaSize, int goalAreaSize) : base(boardWidth, taskAreaSize,
            goalAreaSize)
        {
        }

    }
}