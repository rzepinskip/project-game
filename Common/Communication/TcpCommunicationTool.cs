using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using Common.Interfaces;

namespace Common.Communication
{
    public abstract class TcpCommunicationTool : ICommunicationTool
    {
        public int Id { get; set; }
        public Socket WorkSocket { get; set; }
        public ManualResetEvent MessageProcessed { get; }
        public CommunicationStateObject State { get; set; }

        private IMessageConverter _messageConverter;
        public abstract void Handle(IMessage message, int id = -404);

        public TcpCommunicationTool(Socket workSocket, int id, IMessageConverter messageConverter)
        {
            WorkSocket = workSocket;
            Id = id;
            _messageConverter = messageConverter;
            MessageProcessed = new ManualResetEvent(true);
            State = new CommunicationStateObject();
        }

        public void Receive()
        {
            MessageProcessed.Reset();

            try
            {
                WorkSocket.BeginReceive(State.Buffer, 0, CommunicationStateObject.BufferSize, 0,
                    ReadCallback, State);
            }
            catch (Exception e)
            {
                //After closing socket, BeginReceive will throw SocketException which has to be handled
                Console.WriteLine(e.ToString());
            }

            MessageProcessed.WaitOne();
        }

        private void ReadCallback(IAsyncResult ar)
        {
            var state = (CommunicationStateObject)ar.AsyncState;
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
                var (messages, hasETBbyte) = state.SplitMessages(bytesRead, Id);

                foreach (var message in messages)
                    Handle(_messageConverter.ConvertStringToMessage(message), Id);

                //DONT TOUCH THAT 
                //DANGER ZONE ************
                if (!hasETBbyte)
                    handler.BeginReceive(state.Buffer, 0, CommunicationStateObject.BufferSize, 0,
                        ReadCallback, state);
                else
                    MessageProcessed.Set();
                //DANGER ZONE ****************

            }
        }
        public void Send(byte[] byteData)
        {
            WorkSocket.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, WorkSocket);
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

