using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Common.Communication
{
    public class CommunicationStateObject
    {
        public const int BufferSize = 1024;
        public const char EtbByte = (char)23;
        public byte[] Buffer { get; } =  new byte[BufferSize];
        public StringBuilder Sb { get; } = new StringBuilder();
        public Socket WorkSocket { get; set; }
        public ManualResetEvent MessageProcessed { get; } = new ManualResetEvent(true);
        public long LastMessageReceivedTicks { get; set; }

        public CommunicationStateObject(Socket workSocket)
        {
            WorkSocket = workSocket;
            LastMessageReceivedTicks = DateTime.Now.Ticks;
        }
    }
}
