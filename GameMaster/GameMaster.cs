using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.ActionInfo;
using Common.Communication;
using Common.Interfaces;
using GameMaster.ActionHandlers;
using GameMaster.Configuration;
using NLog;

namespace GameMaster
{
    public class GameMaster : IGameMaster
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public GameMaster(GameConfiguration gameConfiguration)
        {
            GameConfiguration = gameConfiguration;

            var boardGenerator = new GameMasterBoardGenerator();
            Board = boardGenerator.InitializeBoard(GameConfiguration.GameDefinition);

            foreach (var player in Board.Players)
            {
                PlayerGuidToId.Add(Guid.NewGuid().ToString(), player.Key);
            }

            _communicationClient = new AsynchronousClient(new GameMasterConverter());
            _communicationClient.SetupClient(HandleMessagesFromClient);
            new Thread(() => _communicationClient.StartClient()).Start();
        }

        public GameMaster(GameMasterBoard board, Dictionary<string, int> playerGuidToId)
        {
            Board = board;

            PlayerGuidToId = playerGuidToId;
        }

        public Dictionary<int, ObservableConcurrentQueue<IRequest>> RequestsQueues { get; set; } =
            new Dictionary<int, ObservableConcurrentQueue<IRequest>>();

        public Dictionary<int, ObservableConcurrentQueue<IMessage>> ResponsesQueues { get; set; } =
            new Dictionary<int, ObservableConcurrentQueue<IMessage>>();

        public Dictionary<int, bool> IsPlayerQueueProcessed { get; set; } = new Dictionary<int, bool>();
        public Dictionary<int, object> IsPlayerQueueProcessedLock { get; set; } = new Dictionary<int, object>();

        public GameConfiguration GameConfiguration { get; }
        public Dictionary<string, int> PlayerGuidToId { get; } = new Dictionary<string, int>();
        public GameMasterBoard Board { get; set; }

        private readonly IClient _communicationClient;

        public (DataFieldSet data, bool isGameFinished) EvaluateAction(ActionInfo actionInfo)
        {
            var playerId = PlayerGuidToId[actionInfo.PlayerGuid];
            var action = new ActionHandlerDispatcher((dynamic)actionInfo, Board, playerId);
            var responseData = action.Execute();
            var _isGameFinished = Board.IsGameFinished();
            return (data: responseData, isGameFinished: _isGameFinished);
        }

        public virtual event EventHandler<GameFinishedEventArgs> GameFinished;

        public void PutLog(ILoggable record)
        {
            _logger.Info(record.ToLog());
        }

        public void PutActionLog(IRequest record)
        {
            var playerId = PlayerGuidToId[record.PlayerGuid];
            var playerInfo = Board.Players[playerId];
            var actionLog = new RequestLog(record, playerInfo.Team, playerInfo.Role);
            _logger.Info(actionLog.ToLog());
        }


        public PieceGenerator CreatePieceGenerator(GameMasterBoard board)
        {
            return new PieceGenerator(board, GameConfiguration.GameDefinition.ShamProbability);
        }

        private async void HandleMessagesFromPlayer(int playerId)
        {
            var requestQueue = RequestsQueues[playerId];
            while (true)
            {
                lock (IsPlayerQueueProcessedLock[playerId])
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

                var timeSpan = Convert.ToInt32(GameConfiguration.ActionCosts.GetDelayFor(request.GetActionInfo()));
                PutActionLog(request);
                await Task.Delay(timeSpan);

                IMessage response;
                lock (Board.Lock)
                {
                    response = request.Process(this);

                    if (Board.IsGameFinished())
                    {
                        new Thread(() => GameFinished?.Invoke(this, new GameFinishedEventArgs(Board.CheckWinner())))
                            .Start();
                    }
                }
                _communicationClient.Send(response);
                //ResponsesQueues[playerId].Enqueue(response);
            }
        }

        public void StartListeningToRequests()
        {
            foreach (var queue in RequestsQueues.Values)
                queue.ItemEnqueued += (sender, args) =>
                {
                    var playerId = PlayerGuidToId[args.Item.PlayerGuid];

                    lock (IsPlayerQueueProcessedLock[playerId])
                    {
                        if (!IsPlayerQueueProcessed[playerId])
                            Task.Run(() => HandleMessagesFromPlayer(playerId));
                    }
                };
        }

        private void HandleMessagesFromClient(IMessage obj)
        {
            var request = obj as IRequest;
            if (request == null)
                return;

            PlayerGuidToId.TryGetValue(request.PlayerGuid, out var playerId);
            var requestQueue = RequestsQueues[playerId];
            lock (IsPlayerQueueProcessedLock[playerId])
            {
                requestQueue.Enqueue(request);
            }

        }
    }

    public class GameFinishedEventArgs : EventArgs
    {
        public GameFinishedEventArgs(TeamColor winners)
        {
            Winners = winners;
        }

        public TeamColor Winners { get; set; }
    }
}