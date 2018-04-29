using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using Common.Interfaces;

namespace Communication
{
    public abstract class TcpConnection : ITcpConnection
    {
        private readonly IMessageDeserializer _messageDeserializer;

        public TcpConnection(Socket workSocket, int id, IMessageDeserializer messageDeserializer)
        {
            WorkSocket = workSocket;
            Id = id;
            _messageDeserializer = messageDeserializer;
            MessageProcessed = new ManualResetEvent(true);
            State = new CommunicationState();
        }

        public int Id { get; set; }
        public Socket WorkSocket { get; set; }

        public ManualResetEvent MessageProcessed { get; }
        public CommunicationState State { get; set; }

        public void Receive()
        {
            MessageProcessed.Reset();

            try
            {
                WorkSocket.BeginReceive(State.Buffer, 0, CommunicationState.BufferSize, 0,
                    ReadCallback, State);
            }
            catch (Exception e)
            {
                //After closing socket, BeginReceive will throw SocketException which has to be handled
                Console.WriteLine(e.ToString());
            }

            MessageProcessed.WaitOne();
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
                Console.WriteLine(e.ToString());
            }
        }

        public void FinalizeConnect(IAsyncResult ar)
        {
            WorkSocket.EndConnect(ar);
            Debug.WriteLine($"Socket connected to {WorkSocket.RemoteEndPoint}");
        }

        public abstract void Handle(IMessage message, int id = -404);

        private void ReadCallback(IAsyncResult ar)
        {
            var state = ar.AsyncState as CommunicationState;
            var handler = WorkSocket;
            var bytesRead = 0;

            try
            {
                bytesRead = handler.EndReceive(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            if (bytesRead > 0)
            {
                var (messages, hasEtbByte) = state.SplitMessages(bytesRead, Id);

                foreach (var message in messages)
                    Handle(_messageDeserializer.Deserialize(message), Id);

                //DONT TOUCH THAT 
                //DANGER ZONE ************
                if (!hasEtbByte)
                    handler.BeginReceive(state.Buffer, 0, CommunicationState.BufferSize, 0,
                        ReadCallback, state);
                else
                    MessageProcessed.Set();
                // ManualResetEvent (Semaphore) is signaled only when whole message was received,
                // allowing another thread to start evaluating ReadCallback. Otherwise the same thread 
                // continues to read the rest of the message
                //DANGER ZONE ****************
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                var handler = (Socket) ar.AsyncState;
                var bytesSent = handler.EndSend(ar);
                Debug.WriteLine($"Sent {bytesSent} bytes to client.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}