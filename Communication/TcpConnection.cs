using System;
using System.Net.Sockets;
using System.Threading;
using Common;
using Common.Interfaces;
using Communication.Exceptions;

namespace Communication
{
    public abstract class TcpConnection : ITcpConnection
    {
        protected readonly Action<CommunicationException> ConnectionFailureHandler;
        protected readonly IMessageDeserializer MessageDeserializer;

        protected TcpConnection(Socket workSocket, int socketId,
            Action<CommunicationException> connectionFailureHandler,
            IMessageDeserializer messageDeserializer
        )
        {
            WorkSocket = workSocket;
            MessageDeserializer = messageDeserializer;
            ConnectionFailureHandler = connectionFailureHandler;
            SocketId = socketId;
            MessageProcessed = new ManualResetEvent(true);
            State = new CommunicationState();
            ClientType = ClientType.NonInitialized;
        }

        private Socket WorkSocket { get; }

        private ManualResetEvent MessageProcessed { get; }
        private CommunicationState State { get; }
        public ClientType ClientType { get; set; }

        public int SocketId { get; }

        public void Receive()
        {
            MessageProcessed.Reset();

            try
            {
                WorkSocket.BeginReceive(State.Buffer, 0, Constants.BufferSize, 0,
                    ReadCallback, State);
            }
            catch (Exception e)
            {
                ConnectionException.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }

            MessageProcessed.WaitOne();
        }

        public virtual void Send(byte[] byteData)
        {
            try
            {
                WorkSocket.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, WorkSocket);
            }
            catch (Exception e)
            {
                if (e is SocketException socketException &&
                    socketException.SocketErrorCode == SocketError.ConnectionReset)
                {
                    Console.WriteLine($"SEND: socket #{SocketId} is disconnected.");
                    HandleExpectedConnectionError(new CommunicationException("Send error - socket closed", e,
                        CommunicationException.ErrorSeverity.Fatal));
                    return;
                }

                ConnectionException.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }
        }

        public void SendKeepAlive()
        {
            Send(new[] {Convert.ToByte(Constants.EtbByte)});
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
                ConnectionException.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }
        }

        public void FinalizeConnect(IAsyncResult ar)
        {
            WorkSocket.EndConnect(ar);
        }

        public long GetLastMessageReceivedTicks()
        {
            return State.LastMessageReceivedTicks;
        }

        public void UpdateLastMessageTicks()
        {
            State.UpdateLastMessageTicks();
        }

        public abstract void Handle(IMessage message, int socketId = -404);
        public abstract void HandleKeepAliveMessage();

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
                if (e is SocketException socketException &&
                    socketException.SocketErrorCode == SocketError.ConnectionReset)
                {
                    Console.WriteLine("READ: Somebody disconnected - bubbling up exception...");
                    HandleExpectedConnectionError(new CommunicationException("Read error - socket closed", e,
                        CommunicationException.ErrorSeverity.Fatal));
                    return;
                }

                ConnectionException.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }

            if (bytesRead > 0)
            {
                var (messages, hasEtbByte) = state.SplitMessages(bytesRead, SocketId);
                State.UpdateLastMessageTicks();
                var handledKeepAlive = false;
                foreach (var message in messages)
                {
                    if (!string.IsNullOrEmpty(message)) Handle(MessageDeserializer.Deserialize(message), SocketId);
                    HandleKeepAliveMessage();
                }

                //DONT TOUCH THAT 
                //DANGER ZONE ************
                if (!hasEtbByte)
                    try
                    {
                        handler.BeginReceive(state.Buffer, 0, Constants.BufferSize, 0,
                            ReadCallback, state);
                    }
                    catch (Exception e)
                    {
                        ConnectionException.PrintUnexpectedConnectionErrorDetails(e);
                        throw;
                    }
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
            }
            catch (Exception e)
            {
                ConnectionException.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }
        }

        protected void HandleExpectedConnectionError(Exception e)
        {
            e.Data.Add("socketId", SocketId);
            ConnectionFailureHandler(e);
        }
    }
}