using System;
using Common;
using Messaging.Requests;

namespace Messaging.ActionHelpers
{
    public class RequestLog : ILoggable

    {
        public RequestLog(Request message, TeamColor color, PlayerType role)
        {
            Message = message;
            Color = color;
            Role = role;
        }

        public Request Message { get; set; }
        public TeamColor Color { get; set; }
        public PlayerType Role { get; set; }
        public string ToLog()
        {
            return string.Join(',', Message.ToLog(), Color, Role);
        }
    }
}