using System;
using System.Collections.Generic;
using GameMaster.Configuration;
using Shared;
using Shared.BoardObjects;

namespace GameMaster
{
    public class BoardGenerator
    {
        private readonly Random _random = new Random();
        private Board _board;

        public Board InitializeBoard(GameDefinition gameDefinition)
        {
            _board = new Board(gameDefinition.BoardWidth, gameDefinition.TaskAreaLength, gameDefinition.GoalAreaLength);
            PlaceInitialPieces(gameDefinition.InitialNumberOfPieces, gameDefinition.ShamProbability);

            PlaceGoals(gameDefinition.Goals);

            var players = GeneratePlayers(gameDefinition.NumberOfPlayersPerTeam);

            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                _board.Players.Add(i, player);
                _board.Content[player.Location.X, player.Location.Y].PlayerId = i;
            }

            return _board;
        }

        private void PlaceInitialPieces(int count, double shamProbability)
        {
            var taskAreaBottomLeftCorner = new Location(0, _board.GoalAreaSize);
            var taskAreaTopRightCorner = new Location(_board.Width - 1, _board.Height - (_board.GoalAreaSize + 1));
            var locations = GenerateLocationsOnRectangle(count, taskAreaBottomLeftCorner, taskAreaTopRightCorner);

            for (var i = 0; i < count; i++)
            {
                var type = _random.NextDouble() <= shamProbability
                    ? CommonResources.PieceType.Sham
                    : CommonResources.PieceType.Normal;
                var piece = new Piece(i, type);

                var location = locations.Pop();
                var taskField = _board.Content[location.X, location.Y] as TaskField;
                taskField.DistanceToPiece = 0;
                taskField.PieceId = i;

                _board.Pieces.Add(i, piece);
            }
        }

        private void PlaceGoals(List<GoalField> goals)
        {
            foreach (var goal in goals) _board.Content[goal.X, goal.Y] = goal;
        }

        private List<PlayerInfo> GeneratePlayers(int teamPlayerCount)
        {
            var playersCount = 2 * teamPlayerCount;
            var players = new List<PlayerInfo>(playersCount);

            for (var i = 0; i < playersCount; i++)
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
            var redBottomLeftCorner = new Location(0, _board.Height - _board.GoalAreaSize);
            var redTopRightCorner = new Location(_board.Width - 1, _board.Height - 1);
            var redTeamLocations =
                GenerateLocationsOnRectangle(players.Count / 2, redBottomLeftCorner, redTopRightCorner);

            var blueBottomLeftCorner = new Location(0, 0);
            var blueTopRightCorner = new Location(_board.Width - 1, _board.GoalAreaSize - 1);
            var blueTeamLocations =
                GenerateLocationsOnRectangle(players.Count / 2, blueBottomLeftCorner, blueTopRightCorner);

            foreach (var player in players)
                if (player.Team == CommonResources.TeamColour.Red)
                    player.Location = redTeamLocations.Pop();
                else if (player.Team == CommonResources.TeamColour.Blue)
                    player.Location = blueTeamLocations.Pop();
        }

        private Stack<Location> GenerateLocationsOnRectangle(int count, Location bottomLeftCorner,
            Location topRightCorner)
        {
            var randomLocations = new HashSet<Location>();

            for (var i = 0; i < count; i++)
            {
                var location = new Location();

                do
                {
                    var randomX = _random.Next(bottomLeftCorner.X, topRightCorner.X + 1);
                    var randomY = _random.Next(bottomLeftCorner.Y, topRightCorner.Y + 1);
                    location = new Location(randomX, randomY);
                } while (!randomLocations.Add(location));
            }

            return new Stack<Location>(randomLocations);
        }
    }
}