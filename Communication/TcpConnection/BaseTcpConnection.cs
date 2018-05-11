using System;
using System.Net.Sockets;
using System.Threading;
using Common;
using Common.Interfaces;
using Communication.Errors;

namespace Communication.TcpConnection
{
    public abstract class BaseTcpConnection : ITcpConnection
    {
        protected readonly Action<CommunicationException> ConnectionFailureHandler;
        protected readonly IMessageDeserializer MessageDeserializer;
        protected readonly CommunicationState State;

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

        public ClientType ClientType { get; set; }
        public int Id { get; }

        public virtual void FinalizeConnect(IAsyncResult ar)
        {
            Socket.EndConnect(ar);
        }

        public void Send(byte[] byteData)
        {
            try
            {
                Socket.BeginSend(byteData, 0, byteData.Length, 0, FinalizeSend, Socket);
            }
            catch (Exception e)
            {
                if (e is SocketException socketException &&
                    socketException.SocketErrorCode == SocketError.ConnectionReset)
                {
                    HandleExpectedConnectionError(new IdentifiableCommunicationException(Id, "Send error - socket closed", e,
                        CommunicationException.ErrorSeverity.Temporary));
                    return;
                }

                if (e is ObjectDisposedException)
                {
                    HandleExpectedConnectionError(new IdentifiableCommunicationException(Id, "Disposed during Send", e, CommunicationException.ErrorSeverity.Temporary));
                    return;
                }

                ConnectionError.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }
        }

        public virtual void CloseConnection()
        {
            if (Socket == null)
                return;

            try
            {
                Socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception e)
            {
                ConnectionError.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }
            finally
            {
                Socket.Close();
            }
        }

        public void Receive()
        {
            MessageProcessed.Reset();

            try
            {
                Socket.BeginReceive(State.Buffer, 0, Constants.BufferSize, 0,
                    FinalizeReceive, State);
            }
            catch (Exception e)
            {
                if (e is SocketException socketException &&
                    socketException.SocketErrorCode == SocketError.ConnectionReset)
                {
                    HandleExpectedConnectionError(new IdentifiableCommunicationException(Id, "Receive error - socket closed", e,
                        CommunicationException.ErrorSeverity.Temporary));
                    return;
                }
                if (e is ObjectDisposedException)
                {
                    HandleExpectedConnectionError(new IdentifiableCommunicationException(Id, "Disposed during Send", e, CommunicationException.ErrorSeverity.Temporary));
                    return;
                }

                ConnectionError.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }

            MessageProcessed.WaitOne();
        }

        protected virtual void FinalizeSend(IAsyncResult ar)
        {
            try
            {
                var handler = (Socket) ar.AsyncState;
                var bytesSent = handler.EndSend(ar);
            }
            catch (Exception e)
            {
                ConnectionError.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }
        }

        public abstract void Handle(IMessage message, int connectionId = -404);

        protected virtual void FinalizeReceive(IAsyncResult ar)
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
                    HandleExpectedConnectionError(new IdentifiableCommunicationException(Id, "Read error - socket closed", e,
                        CommunicationException.ErrorSeverity.Temporary));
                    return;
                }

                if (e is ObjectDisposedException)
                {
                    throw new IdentifiableCommunicationException(Id, "Disposed during FinalizeReceive", e, CommunicationException.ErrorSeverity.Temporary);
                }

                ConnectionError.PrintUnexpectedConnectionErrorDetails(e);
                throw;
            }

            if (bytesRead > 0)
            {
                var (messages, hasEtbByte) = state.SplitMessages(bytesRead, Id);
                State.UpdateLastReceivedMessageTicks();

                foreach (var message in messages)
                    if (!string.IsNullOrEmpty(message))
                        Handle(MessageDeserializer.Deserialize(message), Id);

                //DONT TOUCH THAT 
                //DANGER ZONE ************
                if (!hasEtbByte)
                    try
                    {
                        handler.BeginReceive(state.Buffer, 0, Constants.BufferSize, 0,
                            FinalizeReceive, state);
                    }
                    catch (Exception e)
                    {
                        ConnectionError.PrintUnexpectedConnectionErrorDetails(e);
                        throw;
                    }
                else
                    MessageProcessed.Set();

                // ManualResetEvent (Semaphore) is signaled only when whole message was received,
                // allowing another thread to start evaluating FinalizeReceive. Otherwise the same thread 
                // continues to read the rest of the message
                //DANGER ZONE ****************
            }
        }

        protected virtual void HandleExpectedConnectionError(CommunicationException e)
        {
            ConnectionFailureHandler(e);
        }
    }
}