using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common.Interfaces;

namespace Common.Communication
{
    public class StateObject
    {
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();

        public Socket workSocket;
    }

    public class AsynchronousClient : IClient
    {
        private const int port = 11000;
        private readonly ManualResetEvent _connectDone =
            new ManualResetEvent(false);
        private readonly ManualResetEvent _receiveDone =
            new ManualResetEvent(false);

        public event Action<string> StringMessageReceivedEvent;

        // The response from the remote device.  
        private string _response = string.Empty;

        private Socket _client;
        public void StartClient()
        {
            try
            {
                var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                var ipAddress = ipHostInfo.AddressList[0];
                var remoteEP = new IPEndPoint(ipAddress, port);

                _client = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                _client.BeginConnect(remoteEP,
                    ConnectCallback, _client);
                _connectDone.WaitOne();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }

            var thread = new Thread(StartReading);
            thread.Start();

        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                // Complete the connection.  
                _client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    _client.RemoteEndPoint);

                // Signal that the connection has been made.  
                _connectDone.Set();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        private void StartReading()
        {
            Console.WriteLine("Client starts reading");
            while (true)
            {
                _receiveDone.Reset();

                Receive(_client);

                _receiveDone.WaitOne();
            }
        }

        public void Receive(Socket socket)
        {

            // Create the state object.  
            var state = new StateObject();

            //according to docs beginreceive can throw an exception
            try
            {
                // Begin receiving the data from the remote device.  
                _client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    ReceiveCallback, state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.  
                var state = (StateObject)ar.AsyncState;
                var client = state.workSocket;
                var bytesRead = 0;

                // Read data from the remote device.  
                try
                {
                    bytesRead = client.EndReceive(ar);
                    Debug.WriteLine("received bytes" + bytesRead);
                }
                catch (SocketException e)
                {
                    //Console.WriteLine(e.ToString());
                    return;
                }

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    if (state.sb.Length > 1) _response = state.sb.ToString();
                    if (_response.IndexOf("<EOF>") > -1)
                    {
                        var messages = _response.Split("<EOF>");
                        var numberOfMessages = messages.Length;
                        var wholeMessages = String.IsNullOrEmpty(messages[numberOfMessages - 1]);

                        for (int i = 0; i < numberOfMessages - 1; ++i)
                        {
                            var message = messages[i];
                            //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                            //message.Length, message);
                            StringMessageReceivedEvent?.Invoke(message);

                        }
                        state.sb.Clear();
                        if (!wholeMessages)
                        {
                            state.sb.Append(messages[numberOfMessages - 1]);
                        }

                        _receiveDone.Set();

                    } else
                    {
                        // Get the rest of the data.  
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            ReceiveCallback, state);
                    }
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        

        public void Send(string data)
        {
            var byteData = Encoding.ASCII.GetBytes(data);
            try
            {
                _client?.BeginSend(byteData, 0, byteData.Length, 0,
                    SendCallback, _client);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }

        }

        public void SetupClient(Action<string> messageHandler)
        {
            this.StringMessageReceivedEvent += messageHandler;
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                var bytesSent = _client.EndSend(ar);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }
    }
}
