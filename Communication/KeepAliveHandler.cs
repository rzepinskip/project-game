using System;
using System.Threading;

namespace Communication
{
    public class KeepAliveHandler
    {
        private Timer _checkKeepAliveTimer;
        protected readonly int _keepAliveTimeInterval;
        protected readonly IKeepAliveGetter _keepAliveGetter;
        public KeepAliveHandler(int keepAliveTimeInterval, IKeepAliveGetter keepAliveGetter)
        {
            _keepAliveGetter = keepAliveGetter;
            _keepAliveTimeInterval = keepAliveTimeInterval / 2;
            _checkKeepAliveTimer = new Timer(KeepAliveCallback, null, 0,
                _keepAliveTimeInterval);
        }

        private void KeepAliveCallback(object state)
        {
            var currentTime = DateTime.Now.Ticks;
            foreach (var csStateObject in _keepAliveGetter.Get())
            {
                var elapsedTicks = currentTime - csStateObject.GetLastMessageReceivedTicks();
                var elapsedSpan = new TimeSpan(elapsedTicks);

                if (elapsedSpan.Milliseconds > _keepAliveTimeInterval)
                    csStateObject.CloseSocket();
            }
        }
    }
}
