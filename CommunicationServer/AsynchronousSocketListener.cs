using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;
using Common.Interfaces;

namespace CommunicationServer
{
    // State object for reading client data asynchronously  
    public class StateObject
    {
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
        public Socket workSocket;
        public ManualResetEvent messageProcessed = new ManualResetEvent(true);
        public int socketID;
    }

    public class AsynchronousSocketListener : ICommunicationServer
    {
        public event Action<IMessage, int> MessageReceivedEvent;
        public ManualResetEvent allDone = new ManualResetEvent(false);

        public Socket GmSocket;
        public Dictionary<int, Socket> _agentToSocket;
        public Dictionary<int, int> _playerIdToGameId;
        public Dictionary<int, GameInfo> _gameIdToGameInfo;

        private int _counter;

        private readonly IMessageConverter _messageConverter;

        public AsynchronousSocketListener(IMessageConverter messageConverter)
        {
            _messageConverter = messageConverter;
            _counter = 0;
        }

        public void StartListening()
        {
            _agentToSocket = new Dictionary<int, Socket>();
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.  
            var listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        AcceptCallback,
                        listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            var listener = (Socket)ar.AsyncState;
            var handler = listener.EndAccept(ar);

            // Create the state object.  
            var state = new StateObject();
            state.workSocket = handler;
            state.socketID = _counter;

            _agentToSocket.Add(_counter++, handler);

            StartReading(state);
        }

        private void StartReading(StateObject state)
        {
            while (true)
            {
                state.messageProcessed.Reset();

                Receive(state);

                state.messageProcessed.WaitOne();
            }
        }

        private void Receive(StateObject state)
        {

            try
            {
                state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    ReadCallback, state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            Console.WriteLine("in callback");
            var content = string.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            var state = (StateObject)ar.AsyncState;
            var handler = state.workSocket;
            var bytesRead = 0;
            // Read data from the client socket.  
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
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the   
                    // client. Display it on the console.  
                    var messages = content.Split("<EOF>");
                    var numberOfMessages = messages.Length;
                    var wholeMessages = String.IsNullOrEmpty(messages[numberOfMessages - 1]);

                    for (int i = 0; i < numberOfMessages - 1; ++i)
                    {
                        var message = messages[i];
                        //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        //    message.Length, message);
                        MessageReceivedEvent?.Invoke(_messageConverter.ConvertStringToMessage(message), state.socketID);

                    }
                    state.sb.Clear();
                    if (!wholeMessages)
                    {
                        state.sb.Append(messages[numberOfMessages - 1]);
                    }

                    // Echo the data back to the client. 
                    state.messageProcessed.Set();
                    //Send(handler, content);
                } else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        ReadCallback, state);
                }
            }
        }

        public void Send(IMessage message, int id)
        {
            var byteData = Encoding.ASCII.GetBytes(_messageConverter.ConvertMessageToString(message));
            _agentToSocket.TryGetValue(id, out var handler);
            try
            {
                //find socket in dictionary

                // Begin sending the data to the remote device.  
                handler?.BeginSend(byteData, 0, byteData.Length, 0,
                    SendCallback, handler);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void SetupServer(Action<IMessage, int> messageHandler)
        {
            MessageReceivedEvent += messageHandler;
        }

        //public void SendToGM(string data)
        //{
        //    var byteData = Encoding.ASCII.GetBytes(data);

        //    // Begin sending the data to the remote device.  
        //    GmSocket.BeginSend(byteData, 0, byteData.Length, 0,
        //        SendCallback, GmSocket);
        //}

        //public void SendToPlayer(int playerID, string data)
        //{
        //    _agentToSocket.TryGetValue(playerID, out var handler);
        //    // Convert the string data to byte data using ASCII encoding.  
        //    var byteData = Encoding.ASCII.GetBytes(data);
        //    try
        //    {
        //        // Begin sending the data to the remote device.  
        //        handler?.BeginSend(byteData, 0, byteData.Length, 0,
        //            SendCallback, handler);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                var handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                var bytesSent = handler.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);



                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}