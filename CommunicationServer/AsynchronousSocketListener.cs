using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;
using Common.Communication;
using Common.Interfaces;

namespace CommunicationServer
{
    public class AsynchronousSocketListener : ICommunicationServer
    {
        public event Action<IMessage, int> MessageReceivedEvent;
        private readonly ManualResetEvent _readyForAccept = new ManualResetEvent(false);

        private readonly Dictionary<int, Socket> _agentToSocket;
        private readonly Dictionary<int, int> _playerIdToGameId;
        private readonly Dictionary<int, GameInfo> _gameIdToGameInfo;
        private readonly Dictionary<int, CommunicationStateObject> _agentToCommunicationStateObject;

        private int _counter;
        private readonly IMessageConverter _messageConverter;
        private readonly int _keepAliveTimeMiliseconds;
        private Timer _checkKeepAliveTimer;

        public AsynchronousSocketListener(IMessageConverter messageConverter, int keepAliveTimeMiliseconds)
        {
            _keepAliveTimeMiliseconds = keepAliveTimeMiliseconds;
            _messageConverter = messageConverter;
            _counter = 0;
            _gameIdToGameInfo = new Dictionary<int, GameInfo>();
            _agentToSocket = new Dictionary<int, Socket>();
            _playerIdToGameId = new Dictionary<int, int>();
            _agentToCommunicationStateObject = new Dictionary<int, CommunicationStateObject>();
            _checkKeepAliveTimer = new Timer(KeepAliveCallback, _agentToCommunicationStateObject, 0, _keepAliveTimeMiliseconds/2);
        }

        public void StartListening()
        {
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, 11000);

            var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    _readyForAccept.Reset();
                    //Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(AcceptCallback, listener);

                    _readyForAccept.WaitOne();
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }

            //Console.WriteLine("\nPress ENTER to continue...");
            //Console.Read();
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            _readyForAccept.Set();

            var listener = (Socket)ar.AsyncState;
            var handler = listener.EndAccept(ar);

            var state = new CommunicationStateObject(handler, _counter);
            _agentToCommunicationStateObject.Add(_counter, state);
            _agentToSocket.Add(_counter++, handler);

            StartReading(state);
        }

        private void StartReading(CommunicationStateObject state)
        {
            while (true)
            {
                state.MessageProcessed.Reset();

                Receive(state);

                state.MessageProcessed.WaitOne();
            }
        }

        private void Receive(CommunicationStateObject state)
        {

            try
            {
                state.WorkSocket.BeginReceive(state.Buffer, 0, CommunicationStateObject.BufferSize, 0,
                    ReadCallback, state);
            }
            catch (Exception e)
            {
                //After closing socket, BeginReceive will throw SocketException which has to be handled
                //Console.WriteLine(e.ToString());
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            //Console.WriteLine("in callback");
            var content = string.Empty;
            var state = (CommunicationStateObject)ar.AsyncState;
            var handler = state.WorkSocket;
            var bytesRead = 0;
            try
            {
                bytesRead = handler.EndReceive(ar);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }

            if (bytesRead > 0)
            {
                state.Sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
                content = state.Sb.ToString();
                if (content.IndexOf(CommunicationStateObject.EtbByte) > -1)
                {
                    var messages = content.Split(CommunicationStateObject.EtbByte);
                    var numberOfMessages = messages.Length;
                    var wholeMessages = string.IsNullOrEmpty(messages[numberOfMessages - 1]);

                    for (var i = 0; i < numberOfMessages - 1; ++i)
                    {
                        var message = messages[i];
                        //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        //    message.Length, message);
                        state.LastMessageReceivedTicks = DateTime.Today.Ticks;
                        MessageReceivedEvent?.Invoke(_messageConverter.ConvertStringToMessage(message), state.SocketId);

                    }
                    state.Sb.Clear();
                    if (!wholeMessages)
                    {
                        state.Sb.Append(messages[numberOfMessages - 1]);
                    }

                    state.MessageProcessed.Set();
                } else
                {
                    handler.BeginReceive(state.Buffer, 0, CommunicationStateObject.BufferSize, 0,
                        ReadCallback, state);
                }
            }
        }

        public void Send(IMessage message, int id)
        {
            var byteData = Encoding.ASCII.GetBytes(_messageConverter.ConvertMessageToString(message) + CommunicationStateObject.EtbByte);
            var findResult = _agentToSocket.TryGetValue(id, out var handler);
            if (!findResult)
            {
                throw new Exception("Non exsistent socket id");
            }
            try
            {
                handler?.BeginSend(byteData, 0, byteData.Length, 0,
                    SendCallback, handler);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                var handler = (Socket)ar.AsyncState;
                var bytesSent = handler.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        public void SetupServer(Action<IMessage, int> messageHandler)
        {
            MessageReceivedEvent += messageHandler;
        }

        public IEnumerable<GameInfo> GetGames()
        {
            return _gameIdToGameInfo.Values;
        }

        public int GetGameId(string gameName)
        {
            return _gameIdToGameInfo.FirstOrDefault(x => x.Value.GameName == gameName).Key;
        }

        public void RegisterNewGame(GameInfo gameInfo, int id)
        {
            _gameIdToGameInfo.Add(id, gameInfo);
        }

        public void UpdateTeamCount(int gameId, TeamColor team)
        {
            _gameIdToGameInfo.TryGetValue(gameId, out var info);
            if (info == null)
                return;
            switch (team)
            {
            case TeamColor.Blue:
                info.BlueTeamPlayers--;
                break;
            case TeamColor.Red:
                info.RedTeamPlayers--;
                break;
            default:
                throw new Exception("Unexpected team color");
            }
            _gameIdToGameInfo[gameId] = info;
        }

        public void UnregisterGame(int gameId)
        {
            _gameIdToGameInfo.Remove(gameId);
        }

        public void AssignGameIdToPlayerId(int gameId, int playerId)
        {
            _playerIdToGameId.Add(playerId, gameId);
        }

        public int GetGameIdForPlayer(int playerId)
        {
            _playerIdToGameId.TryGetValue(playerId, out var gameId);
            return gameId;
        }

        public void KeepAliveCallback(object state)
        {
            var dictionary = (Dictionary<int, CommunicationStateObject>)state;
            var currentTime = DateTime.Now.Ticks;
            foreach (var csStateObject in dictionary.Values)
            {
                var elapsedTicks = currentTime - csStateObject.LastMessageReceivedTicks;
                var elapsedSpan = new TimeSpan(elapsedTicks);

                if (elapsedSpan.Milliseconds > _keepAliveTimeMiliseconds)
                    CloseSocket(csStateObject.SocketId);
            }

        }

        private void CloseSocket(int socketId)
        {
            _agentToSocket.TryGetValue(socketId, out var socketToClose);
            if (socketToClose == null)
                return;
            try
            {
                socketToClose.Shutdown(SocketShutdown.Both);
                socketToClose.Close();
            }
            catch (Exception e)
            {
                //
            }
        }
    }
}