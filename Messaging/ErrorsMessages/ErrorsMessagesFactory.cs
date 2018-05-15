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

        public IMessage CreateErrorMessage(string exceptionType, string exceptionMessage = "",
            string exceptionCauseParameterName = "")
        {
            return new ErrorMessage(exceptionType, exceptionMessage, exceptionCauseParameterName);
        }
    }
}