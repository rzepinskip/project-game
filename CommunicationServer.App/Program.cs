using System;
using Common;
using Messaging.Serialization;
using NLog;

namespace CommunicationServer.App
{
    class Program
    {
        private static ILogger _logger;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var cs = new CommunicationServer(MessageSerializer.Instance);
            _logger = CommunicationServer.Logger;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            UnhandledApplicationException.HandleGlobalException(args, _logger);
        }
    }
}
