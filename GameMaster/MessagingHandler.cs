using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClientsCommon;
using Common;
using Common.Interfaces;
using GameMaster.Configuration;
using GameMaster.Delays;

namespace GameMaster
{
    public class MessagingHandler
    {
        private readonly ActionCosts _actionCosts;
        public readonly ICommunicationClient CommunicationClient;    
        private bool _gameFinished;
        private readonly Action _hostNewGame;
        private Dictionary<Guid, PlayerHandle> _playerHandles;
        private readonly IGameResultsMessageFactory _gameResultsMessageFactory;

        public MessagingHandler(GameConfiguration gameConfiguration, ICommunicationClient communicationCommunicationClient, Action hostNewGame, IGameResultsMessageFactory gameResultsMessageFactory)
        {
            _actionCosts = gameConfiguration.ActionCosts;
            _hostNewGame = hostNewGame;
            _gameResultsMessageFactory = gameResultsMessageFactory;
            CommunicationClient = communicationCommunicationClient;
            new Thread(() => CommunicationClient.Connect(HandleConnectionError, HandleMessagesFromClient)).Start();
        }

        private async void HandleMessagesFromPlayer(Guid playerGuid)
        {
            var playerHandle = _playerHandles[playerGuid];

            while (true)
            {
                lock (playerHandle.Lock)
                {
                    if (playerHandle.Queue.IsEmpty)
                    {
                        playerHandle.IsProcessed = false;
                        break;
                    }

                    playerHandle.IsProcessed = true;
                }

                IRequestMessage requestMessage;
                while (!playerHandle.Queue.TryDequeue(out requestMessage))
                    await Task.Delay(10);

                var timeSpan = Convert.ToInt32(_actionCosts.GetDelayFor(requestMessage.GetActionInfo()));
                await Task.Delay(timeSpan);

                //if (!_playerHandles.ContainsKey(playerGuid))
                //{
                //    Console.WriteLine($"Unrecoginzed player with guid:{playerGuid}");
                //    continue;
                //}

                MessageReceived.Invoke(this, requestMessage);
            }
        }

        public virtual event EventHandler<IMessage> MessageReceived;

        public void StartListeningToRequests(IEnumerable<Guid> playersGuids)
        {
            CreateQueues(playersGuids);

            var queues = _playerHandles.Values.Select(h => h.Queue);

            foreach (var queue in queues)
                queue.ItemEnqueued += (sender, args) =>
                {
                    var playerHandle = _playerHandles[args.Item.PlayerGuid];

                    lock (playerHandle.Lock)
                    {
                        if (!playerHandle.IsProcessed)
                            Task.Run(() => HandleMessagesFromPlayer(args.Item.PlayerGuid));
                    }
                };
        }

        private void HandleMessagesFromClient(IMessage message)
        {
            if (message is IRequestMessage request)
            {
                //if (!_playerHandles.ContainsKey(request.PlayerGuid))
                //{
                //    Console.WriteLine($"Unrecoginzed player with guid:{request.PlayerGuid}");
                //    return;
                //}

                var playerHandle = _playerHandles[request.PlayerGuid];
                lock (playerHandle.Lock)
                {
                    playerHandle.Queue.Enqueue(request);
                }
            }
            else
            {
                MessageReceived.Invoke(this, message);
            }
        }

        public void CreateQueues(IEnumerable<Guid> guids)
        {
            _playerHandles = new Dictionary<Guid, PlayerHandle>();

            foreach (var guid in guids) _playerHandles.Add(guid, new PlayerHandle());
        }

        private class PlayerHandle
        {
            public PlayerHandle()
            {
                Queue = new ObservableConcurrentQueue<IRequestMessage>();
                IsProcessed = false;
                Lock = new object();
            }

            public ObservableConcurrentQueue<IRequestMessage> Queue { get; }
            public bool IsProcessed { get; set; }
            public object Lock { get; }
        }

        public void HandleConnectionError(CommunicationException e)
        {
            CommunicationClient.HandleCommunicationError(e);

            if (e.Severity == CommunicationException.ErrorSeverity.Temporary)
                return;

            new Thread(() => CommunicationClient.Connect(HandleConnectionError, HandleMessagesFromClient)).Start();
            _hostNewGame();

        }

        public void BroadcastGameResults(IEnumerable<BoardData> boardData)
        {
            foreach (var data in boardData)
            {
                CommunicationClient.Send(_gameResultsMessageFactory.CreateGameResultsMessage(data));
            }
        }

        public double KnowledgeExchangeDelay => _actionCosts.KnowledgeExchangeDelay;
    }
}