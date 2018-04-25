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

            //if (bytesRead > 0)
            //{
            //    state.Sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
            //    var content = state.Sb.ToString();
            //    if (content.IndexOf(CommunicationStateObject.EtbByte) > 1)
            //    {
            //        var messages = content.Split(CommunicationStateObject.EtbByte);
            //        var numberOfMessages = messages.Length;
            //        var wholeMessages = string.IsNullOrEmpty(messages[numberOfMessages - 1]);

            //        for (var i = 0; i < numberOfMessages - 1; ++i)
            //        {
            //            var message = messages[i];
            //            Debug.WriteLine("Read {0} bytes from socket. \n Data : {1}",
            //                message.Length, message);
            //            state.LastMessageReceivedTicks = DateTime.Today.Ticks;
            //            Handle(_messageConverter.ConvertStringToMessage(message), Id);
            //        }
            //        state.Sb.Clear();
            //        if (!wholeMessages)
            //        {
            //            state.Sb.Append(messages[numberOfMessages - 1]);
            //        }

            //        MessageProcessed.Set();
            //    }
            //    else
            //    {
            //        handler.BeginReceive(state.Buffer, 0, CommunicationStateObject.BufferSize, 0,
            //            ReadCallback, state);
            //    }
            //}

            if (bytesRead > 0)
            {
                var (messages, hasETBbyte) = state.SplitMessages(bytesRead, Id);

                foreach (var message in messages)
                    Handle(_messageConverter.ConvertStringToMessage(message), Id);

                if (!hasETBbyte)
                    handler.BeginReceive(state.Buffer, 0, CommunicationStateObject.BufferSize, 0,
                        ReadCallback, state);
                else
                    MessageProcessed.Set();

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

