using System;
using Common;

namespace Communication.Errors
{
    public class IdentifiableCommunicationException : CommunicationException
    {
        public int ConnectionId { get; set; }

        public IdentifiableCommunicationException(int connectionId, string message, Exception innerException, ErrorSeverity severity) : base(message, innerException, severity)
        {
            ConnectionId = connectionId;
        }
    }
}
