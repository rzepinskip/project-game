using System;
using System.Threading;

namespace Communication
{
    public class KeepAliveHandler
    {
        private Timer _checkKeepAliveTimer;
        protected readonly int KeepAliveTimeInterval;
        protected readonly IKeepAliveGetter KeepAliveGetter;
        public KeepAliveHandler(int keepAliveTimeInterval, IKeepAliveGetter keepAliveGetter)
        {
            KeepAliveGetter = keepAliveGetter;
            KeepAliveTimeInterval = keepAliveTimeInterval;
            _checkKeepAliveTimer = new Timer(KeepAliveCallback, null, 0,
                KeepAliveTimeInterval/2);
        }

        private void KeepAliveCallback(object state)
        {
            var currentTime = DateTime.Now.Ticks;
            foreach (var csStateObject in KeepAliveGetter.Get())
            {
                var elapsedTicks = currentTime - csStateObject.GetLastMessageReceivedTicks();
                var elapsedSpan = new TimeSpan(elapsedTicks);

                if (elapsedSpan.Milliseconds > KeepAliveTimeInterval)
                    csStateObject.CloseSocket();
            }
        }
    }
}
