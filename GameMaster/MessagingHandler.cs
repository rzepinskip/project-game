﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Interfaces;
using Communication;
using Communication.Client;
using GameMaster.Configuration;
using GameMaster.Delays;

namespace GameMaster
{
    public class MessagingHandler
    {
        private readonly ActionCosts _actionCosts;
        public readonly IClient Client;

        private Dictionary<Guid, PlayerHandle> _playerHandles;

        public MessagingHandler(GameConfiguration gameConfiguration, IMessageDeserializer messageDeserializer, int port, IPAddress address)
        {
            _actionCosts = gameConfiguration.ActionCosts;

            Client = new AsynchronousClient(new TcpSocketConnector(messageDeserializer, HandleMessagesFromClient, port, address));
            new Thread(() => Client.Connect()).Start();
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

                IRequest request;
                while (!playerHandle.Queue.TryDequeue(out request))
                    await Task.Delay(10);

                var timeSpan = Convert.ToInt32(_actionCosts.GetDelayFor(request.GetActionInfo()));
                await Task.Delay(timeSpan);

                MessageReceived.Invoke(this, request);
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
            if (message is IRequest request)
            {
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
                Queue = new ObservableConcurrentQueue<IRequest>();
                IsProcessed = false;
                Lock = new object();
            }

            public ObservableConcurrentQueue<IRequest> Queue { get; }
            public bool IsProcessed { get; set; }
            public object Lock { get; }
        }
    }
}