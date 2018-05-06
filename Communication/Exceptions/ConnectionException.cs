using System;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;

namespace Communication.Exceptions
{
    public class ConnectionException : Exception
    {
        public ConnectionException()
        {
        }

        public ConnectionException(string message) : base(message)
        {
        }

        public ConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static void HandleUnexpectedConnectionError(Exception e)
        {
            var errorMessage = new StringBuilder();

            errorMessage.Append("Unexpected connection error:");

            if (e is SocketException socketException)
                errorMessage.AppendLine($"[{socketException.SocketErrorCode}]");

            errorMessage.AppendLine("\n " + e);

            Console.WriteLine(errorMessage);
        }
    }
}