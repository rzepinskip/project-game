using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Common.Communication
{
    public class CommunicationStateObject
    {
        public const int BufferSize = 1024;
        public const char ETBByte = (char)23;
        public byte[] Buffer { get; } =  new byte[BufferSize];
        public StringBuilder Sb { get; } = new StringBuilder();
        public Socket WorkSocket { get; set; }
        public ManualResetEvent MessageProcessed { get; } = new ManualResetEvent(true);
        public int SocketID { get; }

        public CommunicationStateObject(Socket workSocket, int socketId = 0)
        {
            SocketID = socketId;
            WorkSocket = workSocket;
        }
    }
}
