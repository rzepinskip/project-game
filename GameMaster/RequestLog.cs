using Common;
using Common.Interfaces;

namespace GameMaster
{
    public class RequestLog : ILoggable
    {
        public RequestLog(IRequest message, TeamColor color, PlayerType role)
        {
            Message = message;
            Color = color;
            Role = role;
        }

        public IRequest Message { get; set; }
        public TeamColor Color { get; set; }
        public PlayerType Role { get; set; }
        public string ToLog()
        {
            return string.Join(',', Message.ToLog(), Color, Role);
        }
    }
}