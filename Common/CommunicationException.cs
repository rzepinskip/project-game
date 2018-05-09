using System;

namespace Common
{
    public class CommunicationException : Exception
    {
        public CommunicationException(string message, Exception innerException, ErrorSeverity severity) : base(message, innerException)
        {
            Severity = severity;
        }

        public ErrorSeverity Severity { get; set; }

        public enum ErrorSeverity
        {
            Fatal,
            Temporary
        }
    }
}
