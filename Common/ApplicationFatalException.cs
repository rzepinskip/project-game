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
        
        public static void HandleFatalException(UnhandledExceptionEventArgs args, ILogger logger)
        {
            if (args.ExceptionObject is Exception innerException)
            {
                Console.WriteLine("FATAL ERROR - check logs for more details!");
                Console.WriteLine(args.ExceptionObject.ToString());
                logger.Fatal($"FATAL error: {innerException}");
                Console.ReadKey();
            }
        }
    }
}