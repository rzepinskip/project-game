using System;
using System.Linq;
using System.Threading;

namespace Communication.Client
{
    public class KeepAliveClientHandler : KeepAliveHandler
    {
        private Timer _sendKeepAliveTimer;
       
        public KeepAliveClientHandler(int keepAliveTimeInterval, IKeepAliveGetter keepAliveGetter) : base(keepAliveTimeInterval, keepAliveGetter)
        {
            _sendKeepAliveTimer = new Timer(SendKeepAliveCallback, null, 0, keepAliveTimeInterval/2);
        }

        private void SendKeepAliveCallback(object state)
        {
            KeepAliveGetter.Get().First().SendKeepAlive();
        }

        public void ResetTimer()
        {
            _sendKeepAliveTimer.Change(0, KeepAliveTimeInterval);
        }

    }
}
