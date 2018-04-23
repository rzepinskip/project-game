using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Common;
using Common.ActionInfo;
using Common.Interfaces;
using GameMaster.ActionHandlers;
using GameMaster.Configuration;
using Messaging.InitialisationMessages;
using NLog;

namespace GameMaster
{
    public class GameMaster : IGameMaster
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GameMasterBoardGenerator _gameMasterBoardGenerator;
        private readonly string _name = "game";

        private readonly CommunicationHandler _communicationHandler;
        private readonly List<(TeamColor team, PlayerType role)> _playersSlots;
        private int _gameId;
        private bool _gameInProgress;
        private Timer checkIfFullTeamTimer;

        public GameMaster(GameConfiguration gameConfiguration)
        {
            GameConfiguration = gameConfiguration;

            _gameMasterBoardGenerator = new GameMasterBoardGenerator();
            Board = _gameMasterBoardGenerator.InitializeBoard(GameConfiguration.GameDefinition);
            _playersSlots =
                _gameMasterBoardGenerator.GeneratePlayerSlots(GameConfiguration.GameDefinition.NumberOfPlayersPerTeam);

            PlayerGuidToId = new Dictionary<Guid, int>();
            foreach (var player in Board.Players) PlayerGuidToId.Add(Guid.NewGuid(), player.Key);
            
            checkIfFullTeamTimer = new Timer(CheckIfGameFullCallback, null, 5000, 1000);

            _communicationHandler = new CommunicationHandler(gameConfiguration);
            _communicationHandler.MessageReceived += (sender, args) => MessageHandler(args);

            _communicationHandler.Client.Send(new RegisterGameMessage(new GameInfo(_name,
                GameConfiguration.GameDefinition.NumberOfPlayersPerTeam,
                GameConfiguration.GameDefinition.NumberOfPlayersPerTeam)));
        }

        public GameMaster(GameMasterBoard board, Dictionary<Guid, int> playerGuidToId)
        {
            Board = board;

            PlayerGuidToId = playerGuidToId;
        }

        public GameConfiguration GameConfiguration { get; }
        public Dictionary<Guid, int> PlayerGuidToId { get; }
        public GameMasterBoard Board { get; set; }
        public PieceGenerator PieceGenerator { get; set; }

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
            PlayerGuidToId.Add(playerGuid, playerId);

            return (_gameId, playerGuid, playerInfo);
        }

        public void SetGameId(int gameId)
        {
            _gameId = gameId;
        }

        public (DataFieldSet data, bool isGameFinished) EvaluateAction(ActionInfo actionInfo)
        {
            var playerId = PlayerGuidToId[actionInfo.PlayerGuid];
            var action = new ActionHandlerDispatcher((dynamic) actionInfo, Board, playerId);
            var responseData = action.Execute();
            var _isGameFinished = Board.IsGameFinished();
            return (data: responseData, isGameFinished: _isGameFinished);
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
                _communicationHandler.Client.Send(response);
        }

        private void CheckIfGameFullCallback(object obj)
        {
            if (_playersSlots.Count > 0 || _gameInProgress) return;

            _gameInProgress = true;
            StartNewGame();

            var boardInfo = new BoardInfo(Board.Width, Board.TaskAreaSize, Board.GoalAreaSize);
            foreach (var i in PlayerGuidToId)
            {
                var playerLocation = Board.Players.Values.Single(x => x.Id == i.Value).Location;
                var gameStartMessage = new GameMessage(i.Value, Board.Players.Values, playerLocation, boardInfo);
                _communicationHandler.Client.Send(gameStartMessage);
            }

            _communicationHandler.StartListeningToRequests(PlayerGuidToId.Keys);
        }

        private void StartNewGame()
        {
            var oldBoard = Board;
            var newGmBoardGenerator = new GameMasterBoardGenerator();
            Board = newGmBoardGenerator.InitializeBoard(GameConfiguration.GameDefinition);
            foreach (var boardPlayer in oldBoard.Players)
            {
                var oldPlayerInfo = boardPlayer.Value;
                var playerInfo = new PlayerInfo(oldPlayerInfo.Id, oldPlayerInfo.Team, oldPlayerInfo.Role);
                Board.Players.Add(boardPlayer.Key, playerInfo);
            }
            newGmBoardGenerator.SpawnGameObjects(GameConfiguration.GameDefinition);

            PieceGenerator = new PieceGenerator(Board, GameConfiguration.GameDefinition.ShamProbability, GameConfiguration.GameDefinition.PlacingNewPiecesFrequency);
        }

        private void FinishGame()
        {
            _gameInProgress = false;
            PieceGenerator.SpawnTimer.Dispose();
        }

        public virtual event EventHandler<GameFinishedEventArgs> GameFinished;

        public void PutLog(ILoggable record)
        {
            Logger.Info(record.ToLog());
        }

        public void PutActionLog(IRequest record)
        {
            var playerId = PlayerGuidToId[record.PlayerGuid];
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