using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace Common
{
    public class VerboseLogger
    {
        private ILogger _logger;
        private LoggingMode _mode;

        public VerboseLogger(ILogger logger, LoggingMode mode)
        {
            _logger = logger;
            _mode = mode;
        }

        public void Log(string message)
        {
            _logger.Info(message);
            if(_mode == LoggingMode.Verbose || message == Constants.FatalErrorMessage)
                Console.WriteLine(message);
        }

        public void LogFatal(string message)
        {
            _logger.Fatal(message);
            if (_mode == LoggingMode.Verbose)
                Console.WriteLine(message);
        }

        public void LogException(Exception exception)
        {
            Console.WriteLine(Constants.FatalErrorMessage);
            Console.WriteLine(exception.ToString());
            LogFatal($"FATAL error: {exception}");
        }
    }
}
