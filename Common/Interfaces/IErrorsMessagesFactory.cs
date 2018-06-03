namespace Common.Interfaces
{
    public interface IErrorsMessagesFactory
    {
        IMessage CreatePlayerDisconnectedMessage(int playerId);
        IMessage CreateGameMasterDisconnectedMessage(int gameId);
        IMessage CreateErrorMessage(string exceptionType, string exceptionMessage = "", string exceptionCauseParameterName = "");
    }
}