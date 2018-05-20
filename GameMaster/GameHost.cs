using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Common;
using GameMaster.ActionHandlers;
using GameMaster.Configuration;
using Messaging.InitializationMessages;
using Timer = System.Threading.Timer;

namespace GameMaster
{
    public class GameHost
    {
        public int GameId;
        private readonly GameConfiguration _gameConfiguration;
        private readonly Action _startGame;
        private readonly GameMasterBoardGenerator _boardGenerator;
        public bool GameInProgress;
        private PieceGenerator _pieceGenerator;
        private List<(TeamColor team, PlayerType role)> _playersSlots;
        private List<PlayerInfo> _connectedPlayers;
        private Timer checkIfFullTeamTimer;

        public GameHost(string gameName, GameConfiguration gameConfiguration, Action startGame)
        {
            _gameConfiguration = gameConfiguration;
            _startGame = startGame;
            _gameName = gameName;

            checkIfFullTeamTimer = new Timer(CheckIfGameFullCallback, null,
                (int) Constants.GameFullCheckStartDelay.TotalMilliseconds,
                (int) Constants.GameFullCheckInterval.TotalMilliseconds);

            _boardGenerator = new GameMasterBoardGenerator();
            Board = _boardGenerator.InitializeBoard(gameConfiguration.GameDefinition);
        }

        private readonly string _gameName;

        private void CheckIfGameFullCallback(object obj)
        {
            if (_playersSlots.Count > 0 || GameInProgress) return;

            GameInProgress = true;
            Board = _boardGenerator.InitializeBoard(_gameConfiguration.GameDefinition);
            Board = _boardGenerator.SpawnGameObjects(_gameConfiguration.GameDefinition, _connectedPlayers);
            _pieceGenerator = new PieceGenerator(Board, _gameConfiguration.GameDefinition.ShamProbability,
                _gameConfiguration.GameDefinition.PlacingNewPiecesFrequency);

            _startGame.Invoke();
        }

        public void HostNewGame()
        {
            GameInProgress = false;
            _connectedPlayers = new List<PlayerInfo>();
            _playersSlots =
                GameMasterBoardGenerator.GeneratePlayerSlots(_gameConfiguration.GameDefinition.NumberOfPlayersPerTeam);
            _pieceGenerator?.SpawnTimer.Dispose();
        }

        public GameMasterBoard Board { get; set; }

        public (int gameId, PlayerBase playerInfo) AssignPlayerToAvailableSlotWithPrefered(
            int playerId, TeamColor preferredTeam, PlayerType preferredRole)
        {
            (TeamColor team, PlayerType role) assignedValue;
            if (_playersSlots.Contains((preferredTeam, preferredRole)))
                assignedValue = (preferredTeam, preferredRole);
            else if (_playersSlots.Contains((preferredTeam, PlayerType.Member)))
                assignedValue = (preferredTeam, PlayerType.Member);
            else
                assignedValue = _playersSlots.First();

            _playersSlots.Remove(assignedValue);

            var playerInfo = new PlayerInfo(playerId, assignedValue.team, assignedValue.role);
            _connectedPlayers.Add(playerInfo);

            return (GameId, playerInfo);
        }

        public bool IsSlotAvailable()
        {
            return _playersSlots.Count > 0;
        }

        public GameInfo CurrentGameInfo()
        {
            return new GameInfo(_gameName,
                _gameConfiguration.GameDefinition.NumberOfPlayersPerTeam,
                _gameConfiguration.GameDefinition.NumberOfPlayersPerTeam);
        }
    }
}
