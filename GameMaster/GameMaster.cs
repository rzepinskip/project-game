using System.Collections.Generic;
using System.IO;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;
using CsvHelper;
using Shared.ResponseMessages;
using GameMaster.Configuration;
using System;

namespace GameMaster
{
    public class GameMaster
    {
        public List<Queue<GameMessage>> RequestsQueues { get; set; } = new List<Queue<GameMessage>>();
        public List<Queue<ResponseMessage>> ResponsesQueues { get; set; } = new List<Queue<ResponseMessage>>();
        public Board Board { get; set; }
        Dictionary<string, int> PlayerGuidToId { get; }

        private Random _random = new Random();

        public GameMaster()
        { }

        public void PrepareBoard()
        {
            InitializeBoard("Resources/ExampleConfig.xml");
        }

        private void InitializeBoard(string filePath)
        {
            var configLoader = new ConfigurationLoader();
            var gameConfig = configLoader.LoadConfigurationFromFile(filePath);
            var gameDefinition = gameConfig.GameDefinition;

            Board = new Board(gameDefinition.BoardWidth, gameDefinition.TaskAreaLength, gameDefinition.GoalAreaLength);
            PlaceInitialPieces(gameDefinition.InitialNumberOfPieces, gameDefinition.ShamProbability);

            PlaceGoals(gameDefinition.Goals);

            var players = GeneratePlayers(gameDefinition.NumberOfPlayersPerTeam);

            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                Board.Players.Add(i, player);
                Board.Content[player.Location.X, player.Location.Y].PlayerId = i;
            }
        }

        private void PlaceInitialPieces(int count, double shamProbability)
        {
            var taskAreaBottomLeftCorner = new Location(0, Board.GoalAreaSize);
            var taskAreaTopRightCorner = new Location(Board.Width - 1, Board.Height - (Board.GoalAreaSize + 1));
            var locations = GenerateLocationsOnRectangle(count, taskAreaBottomLeftCorner, taskAreaTopRightCorner);

            for (int i = 0; i < count; i++)
            {
                var type = _random.NextDouble() <= shamProbability ? CommonResources.PieceType.Sham : CommonResources.PieceType.Normal;
                var piece = new Piece(i, type);

                var location = locations.Pop();
                var taskField = Board.Content[location.X, location.Y] as TaskField;
                taskField.DistanceToPiece = 0;
                taskField.PieceId = i;

                Board.Pieces.Add(i, piece);
            }
        }

        private void PlaceGoals(List<GoalField> goals)
        {
            foreach (var goal in goals)
            {
                Board.Content[goal.X, goal.Y] = goal;
            }
        }

        private List<PlayerInfo> GeneratePlayers(int teamPlayerCount)
        {
            var playersCount = 2 * teamPlayerCount;
            var players = new List<PlayerInfo>(playersCount);

            for (int i = 0; i < playersCount; i++)
            {
                var team = i % 2 == 0 ? CommonResources.TeamColour.Red : CommonResources.TeamColour.Blue;
                var role = PlayerBase.PlayerType.Member;

                if (i < 2)
                    role = PlayerBase.PlayerType.Leader;

                var player = new PlayerInfo(team, role, null);  
                players.Add(player);
            }

            AssignStartingLocations(players);

            return players;
        }

        private void AssignStartingLocations(List<PlayerInfo> players)
        {
            var redBottomLeftCorner = new Location(0, Board.Height - Board.GoalAreaSize);
            var redTopRightCorner = new Location(Board.Width - 1, Board.Height-1);
            var redTeamLocations = GenerateLocationsOnRectangle(players.Count / 2, redBottomLeftCorner, redTopRightCorner);

            var blueBottomLeftCorner = new Location(0, 0);
            var blueTopRightCorner = new Location(Board.Width - 1, Board.GoalAreaSize -1);
            var blueTeamLocations = GenerateLocationsOnRectangle(players.Count / 2, blueBottomLeftCorner, blueTopRightCorner);

            foreach (var player in players)
            {
                if (player.Team == CommonResources.TeamColour.Red)
                    player.Location = redTeamLocations.Pop();
                else if (player.Team == CommonResources.TeamColour.Blue)
                    player.Location = blueTeamLocations.Pop();
            }
        }

        private Stack<Location> GenerateLocationsOnRectangle(int count, Location bottomLeftCorner, Location topRightCorner)
        {
            var randomLocations = new HashSet<Location>();

            for (int i = 0; i < count; i++)
            {
                var location = new Location();

                do
                {
                    var randomX = _random.Next(bottomLeftCorner.X, topRightCorner.X + 1);
                    var randomY = _random.Next(bottomLeftCorner.Y, topRightCorner.Y + 1);
                    location = new Location(randomX, randomY);
                }
                while (!randomLocations.Add(location));
            }

            return new Stack<Location>(randomLocations);
        }

        public void PutLog(string filename, ActionLog log)
        {
            using (var textWriter = new StreamWriter(filename, true))
            {
                using (var csvWriter = new CsvWriter(textWriter))
                {
                    csvWriter.NextRecord();
                    csvWriter.WriteRecord(log);
                }
            }
        }
    }
}
