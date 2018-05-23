namespace Common.Interfaces
{
    public interface IGameResultsMessageFactory
    {
        IMessage CreateGameResultsMessage(BoardData boardData);
    }
}
