using System;
using System.Runtime.Serialization;
using NLog;

namespace Common
{
    public class UnhandledApplicationException : Exception
    {
        public UnhandledApplicationException()
        {
        }

        public UnhandledApplicationException(string message) : base(message)
        {
        }

        public UnhandledApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnhandledApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
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
