using Common;
using Common.Interfaces;

namespace Messaging
{
    public class GameResultsMessageFactory : IGameResultsMessageFactory
    {
        public IMessage CreateGameResultsMessage(BoardData boardData)
        {
            return DataMessage.FromBoardData(boardData, true);
        }
    }
}
