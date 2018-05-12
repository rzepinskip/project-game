using System;
using System.Net.Sockets;
using System.Text;

namespace Communication.Errors
{
    public static class ConnectionError
    {
        public static void PrintUnexpectedConnectionErrorDetails(Exception e)
        {
            PrintError(e, string.Empty);
        }

        public static void PrintUnexpectedConnectionErrorDetails(Exception e, int connectionId)
        {
            PrintError(e, $"[conn:{connectionId}] ");
        }

        private static void PrintError(Exception e, string errorDetails)
        {
            var errorMessage = new StringBuilder();
            errorMessage.Append("Unexpected connection error");

            if (!string.IsNullOrWhiteSpace(errorDetails))
                errorMessage.AppendLine(errorDetails);

            if (e is SocketException socketException)
                errorMessage.AppendLine($"[{socketException.SocketErrorCode}]");

            errorMessage.AppendLine("\n " + e);

            Console.WriteLine(errorMessage);
        }
    }
}