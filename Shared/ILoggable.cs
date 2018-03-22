namespace Shared
{
    public interface ILoggable
    {
        ActionLog ToLog(int playerId, PlayerInfo playeInfo);
    }
}