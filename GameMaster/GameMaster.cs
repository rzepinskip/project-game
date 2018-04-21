using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.ActionInfo;
using Common.Communication;
using Common.Interfaces;
using GameMaster.ActionHandlers;
using GameMaster.Configuration;
using Messaging.InitialisationMessages;
using NLog;

namespace GameMaster
{
    public class GameMaster : IGameMaster
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public GameMaster(GameConfiguration gameConfiguration)
        {
            GameConfiguration = gameConfiguration;

            _gameMasterBoardGenerator = new GameMasterBoardGenerator();
            _gameMasterBoardGenerator.InitializePlayersOnBoard(GameConfiguration.GameDefinition);
            Board = _gameMasterBoardGenerator.InitializeBoard(GameConfiguration.GameDefinition);

            foreach (var player in Board.Players)
            {
                PlayerGuidToId.Add(Guid.NewGuid(), player.Key);
            }

            CreateQueues();
            _spawnPieceFrequency = Convert.ToInt32(gameConfiguration.GameDefinition.PlacingNewPiecesFrequency);
            PieceGenerator = CreatePieceGenerator(Board);
            PlayerGuidToQueueId = new Dictionary<Guid, int>();
            checkIfFullTeamTimer = new Timer(CheckIfGameFullCallback, null, 0, 1000);
            _connectedPlayers = 0;
            _communicationClient = new AsynchronousClient(new GameMasterConverter());
            _communicationClient.SetupClient(HandleMessagesFromClient);
            new Thread(() => _communicationClient.StartClient()).Start();

            _communicationClient.Send(new RegisterGameMessage(new GameInfo(_name, GameConfiguration.GameDefinition.NumberOfPlayersPerTeam, GameConfiguration.GameDefinition.NumberOfPlayersPerTeam)));
        }

        public GameMaster(GameMasterBoard board, Dictionary<Guid, int> playerGuidToId)
        {
            Board = board;

            PlayerGuidToId = playerGuidToId;
        }

        private void CreateQueues()
        {
            for(int i = 0; i < Board.Players.Count; ++i)
            {
                var RequestsQueue = new ObservableConcurrentQueue<IRequest>();
                var ResponsesQueue = new ObservableConcurrentQueue<IMessage>();

                RequestsQueues.Add(i, RequestsQueue);
                ResponsesQueues.Add(i, ResponsesQueue);
                IsPlayerQueueProcessed.Add(i, false);
                IsPlayerQueueProcessedLock.Add(i, new object());
            }
        }

        public Dictionary<int, ObservableConcurrentQueue<IRequest>> RequestsQueues { get; set; } =
            new Dictionary<int, ObservableConcurrentQueue<IRequest>>();

        public Dictionary<int, ObservableConcurrentQueue<IMessage>> ResponsesQueues { get; set; } =
            new Dictionary<int, ObservableConcurrentQueue<IMessage>>();

        public Dictionary<int, bool> IsPlayerQueueProcessed { get; set; } = new Dictionary<int, bool>();
        public Dictionary<int, object> IsPlayerQueueProcessedLock { get; set; } = new Dictionary<int, object>();
        public Dictionary<Guid, int> PlayerGuidToQueueId = new Dictionary<Guid, int>();
        public GameConfiguration GameConfiguration { get; }
        public Dictionary<Guid, int> PlayerGuidToId { get; } = new Dictionary<Guid, int>();
        public GameMasterBoard Board { get; set; }
        
        private readonly IClient _communicationClient;

        /////
        private bool _gameRegistered = false;
        private bool _gameStarted = false;
        private int _gameId;
        private int _connectedPlayers;
        private string _name = "game";
        private Timer checkIfFullTeamTimer;
        private GameMasterBoardGenerator _gameMasterBoardGenerator;
        private Thread _pieceGeneratorThread;
        private int _spawnPieceFrequency;
        public PieceGenerator PieceGenerator { get; }
        public bool IsLeaderInTeam(TeamColor team)
        {
            return Board.Players.Values.Any(x => x.Role == PlayerType.Leader && x.Team == team);
        }

        public bool IsPlaceOnTeam(TeamColor team)
        {
            return Board.Players.Values.Any(x => x.Id < 0 && x.Team == team);
        }

        public void AssignPlayerToTeam(int playerId, TeamColor team, PlayerType playerType)
        {
            var firstEmptyGuid = PlayerGuidToId.First(x => x.Value < 0);
            var playerInfoKey = Board.Players.First(x => x.Value.Id < 0 && x.Value.Team == team).Key;
            var oldPlayerInfo = Board.Players[playerInfoKey];
            Board.Players.Remove(playerInfoKey);
            var playerInfo = new PlayerInfo(playerId, team, playerType, oldPlayerInfo.Location);
            Board.Players.Add(playerId, playerInfo);
            //oldPlayerInfo.Id = playerId;
            //oldPlayerInfo.Role = playerType;
            //Board.Players[playerInfoKey] = oldPlayerInfo;
            PlayerGuidToId[firstEmptyGuid.Key] = playerId;
            PlayerGuidToQueueId.Add(firstEmptyGuid.Key, PlayerGuidToQueueId.Count);
            _connectedPlayers++;
            _communicationClient.Send(new ConfirmJoiningGameMessage(_gameId, playerId, firstEmptyGuid.Key, playerInfo));
        }

        public void SetGameId(int gameId)
        {
            _gameId = gameId;
            _gameRegistered = true;
        }

        public void CheckIfGameFullCallback(object obj)
        {
            if (_connectedPlayers == Board.Players.Count && !_gameStarted)
            {
                _gameStarted = true;
                var players = new List<PlayerBase>();
                foreach (var player in Board.Players.Values)
                {
                    players.Add(new PlayerBase(player.Id, player.Team, player.Role));
                }

                var boardInfo = new BoardInfo(Board.Width, Board.GoalAreaSize, Board.TaskAreaSize);

                GenerateNewBoard();

                foreach (var i in PlayerGuidToId)
                {
                    var playerLocation = Board.Players.Values.First(x => x.Id == i.Value).Location;
                    var gameStartMessage = new GameMessage(i.Value, players, playerLocation ,boardInfo);
                    _communicationClient.Send(gameStartMessage);
                }

                _pieceGeneratorThread = new Thread(() => PieceGeneratorGameplay(PieceGenerator));
                _pieceGeneratorThread.Start();
                StartListeningToRequests();
            }
        }

        private void GenerateNewBoard()
        {
            _gameMasterBoardGenerator.InitializeGameObjects(GameConfiguration.GameDefinition);
        }

        private void FinishGame()
        {
            GenerateNewBoard();
            _gameStarted = false;
        }

        private void PieceGeneratorGameplay(PieceGenerator pieceGenerator)
        {
            while (_gameStarted)
            {

                Thread.Sleep(_spawnPieceFrequency);
                pieceGenerator.SpawnPiece();
            }
        }
        /////


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
                        FinishGame();
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
                    var playerId = PlayerGuidToQueueId[args.Item.PlayerGuid];

                    lock (IsPlayerQueueProcessedLock[playerId])
                    {
                        if (!IsPlayerQueueProcessed[playerId])
                            Task.Run(() => HandleMessagesFromPlayer(playerId));
                    }
                };
        }

        private void HandleMessagesFromClient(IMessage obj)
        {
            if (_gameStarted)
            {
                var request = obj as IRequest;
                if (request == null)
                    return;

                PlayerGuidToQueueId.TryGetValue(request.PlayerGuid, out var playerId);
                var requestQueue = RequestsQueues[playerId];
                lock (IsPlayerQueueProcessedLock[playerId])
                {
                    requestQueue.Enqueue(request);
                }
            }
            else if (!_gameStarted)
            {
                obj.Process(this, 0);
            }
            else
            {
                obj.Process(this);
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