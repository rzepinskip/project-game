using Common;

namespace Messaging.ActionHelpers
{
    public interface ILoggable
    {
        ActionLog ToLog(int playerId, PlayerInfo playeInfo);
    }
}