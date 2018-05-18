using Common;
using Common.Interfaces;

namespace GameMaster
{
    public class RequestLog : ILoggable
    {
        public RequestLog(IRequestMessage message, int playerId,TeamColor color, PlayerType role)
        {
            Message = message;
            PlayerId = playerId;
            Color = color;
            Role = role;
        }

        public IRequestMessage Message { get; set; }
        public int PlayerId { get; set; }
        public TeamColor Color { get; set; }
        public PlayerType Role { get; set; }

        public string ToLog()
        {
            return string.Join(',', Message.ToLog(), PlayerId, Message.PlayerGuid, Color, Role);
        }
    }
}