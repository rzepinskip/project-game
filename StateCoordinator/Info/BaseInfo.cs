using Common.Interfaces;
using PlayerStateCoordinator.States;

namespace PlayerStateCoordinator.Info
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