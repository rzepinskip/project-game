using System;
using System.Net.Sockets;
using System.Threading;
using Common;
using Common.Interfaces;
using Communication.Exceptions;

namespace Communication.TcpConnection
{
    public abstract class BaseTcpConnection : ITcpConnection
    {
        protected readonly Action<CommunicationException> ConnectionFailureHandler;
        protected readonly IMessageDeserializer MessageDeserializer;

        protected BaseTcpConnection(int id, Socket socket,
            Action<CommunicationException> connectionFailureHandler,
            IMessageDeserializer messageDeserializer)
        {
            Id = id;
            Socket = socket;
            MessageDeserializer = messageDeserializer;
            ConnectionFailureHandler = connectionFailureHandler;
            MessageProcessed = new ManualResetEvent(true);
            State = new CommunicationState();
            ClientType = ClientType.NonInitialized;
        }

        private Socket Socket { get; }

        private ManualResetEvent MessageProcessed { get; }
        private CommunicationState State { get; }
        public ClientType ClientType { get; set; }

        public int Id { get; }

        public void Receive()
        {
            MessageProcessed.Reset();

            try
            {
                Socket.BeginReceive(State.Buffer, 0, Constants.BufferSize, 0,
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
                Socket.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, Socket);
            }
            catch (Exception e)
            {
                if (e is SocketException socketException &&
                    socketException.SocketErrorCode == SocketError.ConnectionReset)
                {
                    Console.WriteLine($"SEND: socket #{Id} is disconnected.");
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
            if (Socket == null)
                return;

            try
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
            }
            catch (Exception e)
            {
                ConnectionException.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }
        }

        public virtual void FinalizeConnect(IAsyncResult ar)
        {
            Socket.EndConnect(ar);
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
            var handler = Socket;
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
                var (messages, hasEtbByte) = state.SplitMessages(bytesRead, Id);
                State.UpdateLastMessageTicks();
                var handledKeepAlive = false;
                foreach (var message in messages)
                {
                    if (!string.IsNullOrEmpty(message)) Handle(MessageDeserializer.Deserialize(message), Id);
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

        protected virtual void HandleExpectedConnectionError(CommunicationException e)
        {
            e.Data.Add("socketId", Id);
            ConnectionFailureHandler(e);
        }
    }
}