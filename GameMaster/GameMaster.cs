using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using Common.ActionInfo;
using Common.Interfaces;
using Communication;
using GameMaster.ActionHandlers;
using GameMaster.Configuration;
using Messaging.InitialisationMessages;
using NLog;

namespace GameMaster
{
    public class GameMaster : IGameMaster
    {
        private const string Name = "game";
        public static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly GameConfiguration _gameConfiguration;

        private readonly MessagingHandler _messagingHandler;
        private readonly Dictionary<Guid, int> _playerGuidToId;
        private readonly List<(TeamColor team, PlayerType role)> _playersSlots;
        private int _gameId;
        private bool _gameInProgress;
        private PieceGenerator _pieceGenerator;
        private Timer checkIfFullTeamTimer;

        public GameMaster(GameConfiguration gameConfiguration, IMessageDeserializer messageDeserializer)
        {
            _gameConfiguration = gameConfiguration;

            var boardGenerator = new GameMasterBoardGenerator();
            Board = boardGenerator.InitializeBoard(_gameConfiguration.GameDefinition);
            _playersSlots =
                boardGenerator.GeneratePlayerSlots(_gameConfiguration.GameDefinition.NumberOfPlayersPerTeam);

            _playerGuidToId = new Dictionary<Guid, int>();
            foreach (var player in Board.Players) _playerGuidToId.Add(Guid.NewGuid(), player.Key);

            checkIfFullTeamTimer = new Timer(CheckIfGameFullCallback, null, 2000, 1000);

            _messagingHandler = new MessagingHandler(gameConfiguration, messageDeserializer);
            _messagingHandler.MessageReceived += (sender, args) => MessageHandler(args);

            _messagingHandler.Client.Send(new RegisterGameMessage(new GameInfo(Name,
                _gameConfiguration.GameDefinition.NumberOfPlayersPerTeam,
                _gameConfiguration.GameDefinition.NumberOfPlayersPerTeam)));
        }

        public GameMaster(GameMasterBoard board, Dictionary<Guid, int> playerGuidToId)
        {
            Board = board;

            _playerGuidToId = playerGuidToId;
        }

        public GameMasterBoard Board { get; private set; }

        public bool IsSlotAvailable()
        {
            return _playersSlots.Count > 0;
        }

        public (int gameId, Guid playerGuid, PlayerBase playerInfo) AssignPlayerToAvailableSlotWithPrefered(
            int playerId, TeamColor preferedTeam, PlayerType preferedRole)
        {
            (TeamColor team, PlayerType role) assignedValue;
            if (_playersSlots.Contains((preferedTeam, preferedRole)))
                assignedValue = (preferedTeam, preferedRole);
            else if (_playersSlots.Contains((preferedTeam, PlayerType.Member)))
                assignedValue = (preferedTeam, PlayerType.Member);
            else
                assignedValue = _playersSlots.First();

            _playersSlots.Remove(assignedValue);

            var playerInfo = new PlayerInfo(playerId, assignedValue.team, assignedValue.role);
            Board.Players.Add(playerId, playerInfo);

            var playerGuid = Guid.NewGuid();
            _playerGuidToId.Add(playerGuid, playerId);

            return (_gameId, playerGuid, playerInfo);
        }

        public void SetGameId(int gameId)
        {
            _gameId = gameId;
        }

        public (DataFieldSet data, bool isGameFinished) EvaluateAction(ActionInfo actionInfo)
        {
            var playerId = _playerGuidToId[actionInfo.PlayerGuid];
            var action = new ActionHandlerDispatcher((dynamic) actionInfo, Board, playerId);
            return (data: action.Execute(), isGameFinished: Board.IsGameFinished());
        }

        public void MessageHandler(IMessage message)
        {
            // TODO Log all messages
            if (message is IRequest request)
                PutActionLog(request);

            IMessage response;
            lock (Board.Lock)
            {
                response = message.Process(this);
            }

            if (_gameInProgress && Board.IsGameFinished())
            {
                GameFinished?.Invoke(this, new GameFinishedEventArgs(Board.CheckWinner()));
                FinishGame();
            }

            if (response != null)
                _messagingHandler.Client.Send(response);
        }

        private void CheckIfGameFullCallback(object obj)
        {
            if (_playersSlots.Count > 0 || _gameInProgress) return;

            _gameInProgress = true;
            StartNewGame();

            var boardInfo = new BoardInfo(Board.Width, Board.TaskAreaSize, Board.GoalAreaSize);

            _messagingHandler.StartListeningToRequests(_playerGuidToId.Keys);
            foreach (var i in _playerGuidToId)
            {
                var playerLocation = Board.Players.Values.Single(x => x.Id == i.Value).Location;
                var gameStartMessage = new GameMessage(i.Value, Board.Players.Values, playerLocation, boardInfo);
                _messagingHandler.Client.Send(gameStartMessage);
            }
        }

        private void StartNewGame()
        {
            var oldBoard = Board;
            var boardGenerator = new GameMasterBoardGenerator();
            Board = boardGenerator.InitializeBoard(_gameConfiguration.GameDefinition);
            foreach (var boardPlayer in oldBoard.Players)
            {
                var oldPlayerInfo = boardPlayer.Value;
                var playerInfo = new PlayerInfo(oldPlayerInfo.Id, oldPlayerInfo.Team, oldPlayerInfo.Role);
                Board.Players.Add(boardPlayer.Key, playerInfo);
            }

            boardGenerator.SpawnGameObjects(_gameConfiguration.GameDefinition);

            _pieceGenerator = new PieceGenerator(Board, _gameConfiguration.GameDefinition.ShamProbability,
                _gameConfiguration.GameDefinition.PlacingNewPiecesFrequency);
        }

        private void FinishGame()
        {
            _gameInProgress = false;
            _pieceGenerator.SpawnTimer.Dispose();
        }

        public virtual event EventHandler<GameFinishedEventArgs> GameFinished;

        public void PutLog(ILoggable record)
        {
            Logger.Info(record.ToLog());
        }

        public void PutActionLog(IRequest record)
        {
            var playerId = _playerGuidToId[record.PlayerGuid];
            var playerInfo = Board.Players[playerId];
            var actionLog = new RequestLog(record, playerInfo.Team, playerInfo.Role);
            Logger.Info(actionLog.ToLog());
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