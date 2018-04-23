using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                var handler = (Socket)ar.AsyncState;
                var bytesSent = handler.EndSend(ar);
                Debug.WriteLine("Sent {0} bytes to client.", bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Send(byte[] byteData)
        {
            WorkSocket.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, WorkSocket);
        }

        public void CloseSocket()
        {
            if (WorkSocket == null)
                return;
            try
            {
                WorkSocket.Shutdown(SocketShutdown.Both);
                WorkSocket.Close();
            }
            catch (Exception e)
            {
                //
            }
        }
    }
}
