using Common.Interfaces;

namespace Messaging.ErrorsMessages
{
    public class ErrorsMessagesFactory : IErrorsMessagesFactory
    {
        public IMessage CreatePlayerDisconnectedMessage(int playerId)
        {
            return new PlayerDisconnected(playerId);
        }

        public IMessage CreateGameMasterDisconnectedMessage(int gameId)
        {
            return new GameMasterDisconnected(gameId);
        }
    }
}