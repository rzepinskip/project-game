using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.ActionInfo;
using Common.Logging;
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

            var boardGenerator = new BoardGenerator();
            Board = boardGenerator.InitializeBoard(GameConfiguration.GameDefinition);

            foreach (var player in Board.Players)
            {
                PlayerGuidToId.Add(Guid.NewGuid().ToString(), player.Key);
            }
        }

        public Dictionary<int, ObservableConcurrentQueue<Request>> RequestsQueues { get; set; } =
            new Dictionary<int, ObservableConcurrentQueue<Request>>();

        public Dictionary<int, ObservableConcurrentQueue<Response>> ResponsesQueues { get; set; } =
            new Dictionary<int, ObservableConcurrentQueue<Response>>();

        public Dictionary<int, bool> IsPlayerQueueProcessed { get; set; } = new Dictionary<int, bool>();
        public Dictionary<int, object> IsPlayerQueueProcessedLock { get; set; } = new Dictionary<int, object>();

        public GameConfiguration GameConfiguration { get; }
        public Dictionary<string, int> PlayerGuidToId { get; } = new Dictionary<string, int>();
        public GameMasterBoard Board { get; set; }

        public (DataFieldSet data, bool isGameFinished) EvaluateAction(ActionInfo actionInfo)
        {
            var playerId = PlayerGuidToId[actionInfo.PlayerGuid];
            var action = new ActionHandlerDispatcher((dynamic)actionInfo, Board, playerId);
            var responseData = action.Execute();
            var _isGameFinished = IsGameFinished();
            return (data: responseData, isGameFinished: _isGameFinished);
        }

        public virtual event EventHandler<GameFinishedEventArgs> GameFinished;

        public void PutLog(ILoggable record)
        {
            _logger.Info(record.ToLog());
        }

        public void PutActionLog(Request record)
        {
            var playerInfo = Board.Players[record.PlayerId];
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

                Request request;
                while (!requestQueue.TryDequeue(out request))
                    await Task.Delay(10);

                var timeSpan = Convert.ToInt32(request.GetDelay(GameConfiguration.ActionCosts));
                PutActionLog(request);
                await Task.Delay(timeSpan);

                Response response;
                lock (Board.Lock)
                {
                    response = request.Execute(Board);

                    if (Board.IsGameFinished())
                    {
                        new Thread(() => GameFinished?.Invoke(this, new GameFinishedEventArgs(Board.CheckWinner())))
                            .Start();
                        response.IsGameFinished = true;
                    }
                }
                
                // TODO Event handler
            }
        }

        public void StartListeningToRequests()
        {
            foreach (var queue in RequestsQueues.Values)
                queue.ItemEnqueued += (sender, args) =>
                {
                    var playerId = args.Item.PlayerId;

                    lock (IsPlayerQueueProcessedLock[playerId])
                    {
                        if (!IsPlayerQueueProcessed[playerId])
                            Task.Run(() => HandleMessagesFromPlayer(playerId));
                    }
                };
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