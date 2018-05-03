using System;
using System.Runtime.Serialization;
using NLog;

namespace Common
{
    public class GlobalException : Exception
    {
        public GlobalException()
        {
        }

        public GlobalException(string message) : base(message)
        {
        }

        public GlobalException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GlobalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }


        public static void HandleGlobalException(UnhandledExceptionEventArgs args, ILogger logger)
        {
            if (args.ExceptionObject is Exception innerException)
            {
                logger.Fatal($"FATAL error: {innerException}");
            }
                
        }
    }

}
