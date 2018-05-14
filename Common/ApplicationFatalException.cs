using System;
using System.Runtime.Serialization;
using System.Threading;
using NLog;

namespace Common
{
    public class ApplicationFatalException : Exception
    {
        public ApplicationFatalException()
        {
        }

        public ApplicationFatalException(string message) : base(message)
        {
        }

        public ApplicationFatalException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApplicationFatalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        
        public static void HandleFatalException(UnhandledExceptionEventArgs args, VerboseLogger logger)
        {
            if (args.ExceptionObject is Exception innerException)
            {
                logger.LogException(innerException);
                Console.ReadKey();
            }
        }
    }
}