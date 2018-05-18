using System;
using System.Runtime.Serialization;

namespace PlayerStateCoordinator
{
    public class StrategyException : Exception
    {
        public StrategyException()
        {
        }

        public StrategyException(string message) : base(message)
        {
        }

        public StrategyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StrategyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}