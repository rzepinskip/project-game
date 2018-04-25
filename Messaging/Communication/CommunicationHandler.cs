using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common.Communication;
using Common.Interfaces;

namespace Messaging.Communication
{
    public abstract class CommunicationHandler
    {
        public int Id { get; set; }
        public Socket WorkSocket { get; set; }
        public ManualResetEvent MessageProcessed { get; } = new ManualResetEvent(true);
        public CommunicationStateObject State { get; set; }

        private IMessageConverter _messageConverter;

        public CommunicationHandler(Socket workSocket, int id, IMessageConverter messageConverter)
        {
            WorkSocket = workSocket;
            Id = id;
            _messageConverter = messageConverter;
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
                var (messages, lastMessageWaiting) = state.SplitMessages(bytesRead);
                MessageProcessed.Set();

                foreach (var message in messages)
                    Handle(_messageConverter.ConvertStringToMessage(message), Id);

                if (lastMessageWaiting)
                    handler.BeginReceive(state.Buffer, 0, CommunicationStateObject.BufferSize, 0,
                        ReadCallback, state);
            }
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

        public abstract void Handle(IMessage message, int id = -404);


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

