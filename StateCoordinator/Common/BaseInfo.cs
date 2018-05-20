using Common.Interfaces;
using PlayerStateCoordinator.Common.States;

namespace PlayerStateCoordinator.Common
{
    public abstract class BaseInfo : ILoggable
    {
        public State PlayerGameStrategyBeginningState { get; set; }

        public string ToLog()
        {
            return ToString();
        }
    }
}