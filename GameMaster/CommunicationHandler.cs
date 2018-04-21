using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Communication;
using Common.Interfaces;
using GameMaster.Configuration;
using GameMaster.Delays;

namespace GameMaster
{
    public class CommunicationHandler
    {
        private readonly ActionCosts _actionCosts;
        public readonly IClient Client;

        public CommunicationHandler(GameConfiguration gameConfiguration)
        {
            CreateQueues(2 * gameConfiguration.GameDefinition.NumberOfPlayersPerTeam);
            _actionCosts = gameConfiguration.ActionCosts;

            Client = new AsynchronousClient(new GameMasterConverter());
            Client.SetupClient(HandleMessagesFromClient);
            new Thread(() => Client.StartClient()).Start();
        }

        public Dictionary<int, ObservableConcurrentQueue<IRequest>> RequestsQueues { get; private set; }
        public Dictionary<int, ObservableConcurrentQueue<IMessage>> ResponsesQueues { get; private set; }
        public Dictionary<int, bool> IsPlayerQueueProcessed { get; set; }
        public Dictionary<int, object> IsPlayerQueueProcessedLocks { get; set; }

        public Dictionary<Guid, int> PlayerGuidToQueueId { get; private set; }

        private async void HandleMessagesFromPlayer(int playerId)
        {
            var requestQueue = RequestsQueues[playerId];
            while (true)
            {
                lock (IsPlayerQueueProcessedLocks[playerId])
                {
                    if (requestQueue.IsEmpty)
                    {
                        IsPlayerQueueProcessed[playerId] = false;
                        break;
                    }

                    IsPlayerQueueProcessed[playerId] = true;
                }

                IRequest request;
                while (!requestQueue.TryDequeue(out request))
                    await Task.Delay(10);

                var timeSpan = Convert.ToInt32(_actionCosts.GetDelayFor(request.GetActionInfo()));
                await Task.Delay(timeSpan);

                MessageReceived.Invoke(this, request);
            }
        }

        public virtual event EventHandler<IMessage> MessageReceived;

        public void StartListeningToRequests()
        {
            foreach (var queue in RequestsQueues.Values)
                queue.ItemEnqueued += (sender, args) =>
                {
                    var playerId = PlayerGuidToQueueId[args.Item.PlayerGuid];

                    lock (IsPlayerQueueProcessedLocks[playerId])
                    {
                        if (!IsPlayerQueueProcessed[playerId])
                            Task.Run(() => HandleMessagesFromPlayer(playerId));
                    }
                };
        }

        private void HandleMessagesFromClient(IMessage message)
        {
            if (message is IRequest request)
            {
                PlayerGuidToQueueId.TryGetValue(request.PlayerGuid, out var playerId);
                var requestQueue = RequestsQueues[playerId];
                lock (IsPlayerQueueProcessedLocks[playerId])
                {
                    requestQueue.Enqueue(request);
                }
            }
            else
            {
                MessageReceived.Invoke(this, message);
            }
        }


        private void CreateQueues(int playersCount)
        {
            RequestsQueues = new Dictionary<int, ObservableConcurrentQueue<IRequest>>();
            ResponsesQueues = new Dictionary<int, ObservableConcurrentQueue<IMessage>>();

            IsPlayerQueueProcessed = new Dictionary<int, bool>();
            IsPlayerQueueProcessedLocks = new Dictionary<int, object>();
            PlayerGuidToQueueId = new Dictionary<Guid, int>();

            for (var i = 0; i < playersCount; ++i)
            {
                var requestsQueue = new ObservableConcurrentQueue<IRequest>();
                var responsesQueue = new ObservableConcurrentQueue<IMessage>();

                RequestsQueues.Add(i, requestsQueue);
                ResponsesQueues.Add(i, responsesQueue);
                IsPlayerQueueProcessed.Add(i, false);
                IsPlayerQueueProcessedLocks.Add(i, new object());
            }
        }
    }
}