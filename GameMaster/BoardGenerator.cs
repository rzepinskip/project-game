using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.BoardObjects;
using GameMaster.Configuration;

namespace GameMaster
{
    public class BoardGenerator
    {
        private readonly Random _random = new Random();
        protected GameMasterBoard Board;

        public GameMasterBoard InitializeBoard(GameDefinition gameDefinition)
        {
            Board = new GameMasterBoard(gameDefinition.BoardWidth, gameDefinition.TaskAreaLength,
                gameDefinition.GoalAreaLength);

            var pieces = GeneratePieces(gameDefinition.InitialNumberOfPieces, gameDefinition.ShamProbability);
            var piecesLocations = GenerateLocationsForPieces(gameDefinition.InitialNumberOfPieces);
            PlacePieces(pieces, piecesLocations);

            PlaceGoals(gameDefinition.Goals);

            var players = GeneratePlayers(gameDefinition.NumberOfPlayersPerTeam);
            var playersLocations = GenerateLocationsForPlayers(gameDefinition.NumberOfPlayersPerTeam);
            PlacePlayers(players, playersLocations);

            return Board;
        }

        protected List<Piece> GeneratePieces(int count, double shamProbability)
        {
            var pieces = new List<Piece>(count);
            for (var i = 0; i < count; i++)
            {
                var type = _random.NextDouble() <= shamProbability
                    ? PieceType.Sham
                    : PieceType.Normal;
                var piece = new Piece(i, type);

                pieces.Add(piece);
            }

            return pieces;
        }

        protected List<Location> GenerateLocationsForPieces(int count)
        {
            var taskAreaBottomLeftCorner = new Location(0, Board.GoalAreaSize);
            var taskAreaTopRightCorner = new Location(Board.Width - 1, Board.Height - (Board.GoalAreaSize + 1));
            var locations = GenerateLocationsOnRectangle(count, taskAreaBottomLeftCorner, taskAreaTopRightCorner);

            return locations;
        }

        protected void PlacePieces(List<Piece> pieces, List<Location> locations)
        {
            for (var i = 0; i < locations.Count(); i++)
            {
                var taskFieldToFill = Board[locations[i]] as TaskField;
                taskFieldToFill.DistanceToPiece = 0;
                taskFieldToFill.PieceId = pieces[i].Id;

                Board.Pieces.Add(i, pieces[i]);
            }
        }

        protected void PlaceGoals(IEnumerable<GoalField> goals)
        {
            foreach (var goal in goals) Board[goal] = goal;
        }

        protected Dictionary<TeamColor, List<Location>> GenerateLocationsForPlayers(int teamPlayerCount)
        {
            var redBottomLeftCorner = new Location(0, Board.Height - Board.GoalAreaSize);
            var redTopRightCorner = new Location(Board.Width - 1, Board.Height - 1);
            var redTeamLocations =
                GenerateLocationsOnRectangle(teamPlayerCount, redBottomLeftCorner, redTopRightCorner);

            var blueBottomLeftCorner = new Location(0, 0);
            var blueTopRightCorner = new Location(Board.Width - 1, Board.GoalAreaSize - 1);
            var blueTeamLocations =
                GenerateLocationsOnRectangle(teamPlayerCount, blueBottomLeftCorner, blueTopRightCorner);

            var playersLocations = new Dictionary<TeamColor, List<Location>>
            {
                {TeamColor.Blue, blueTeamLocations},
                {TeamColor.Red, redTeamLocations}
            };

            return playersLocations;
        }

        protected List<PlayerInfo> GeneratePlayers(int teamPlayerCount)
        {
            var playersCount = 2 * teamPlayerCount;
            var players = new List<PlayerInfo>(playersCount);

            for (var i = 0; i < playersCount; i++)
            {
                var team = i % 2 == 0 ? TeamColor.Red : TeamColor.Blue;
                var role = PlayerType.Member;

                if (i < 2)
                    role = PlayerType.Leader;

                var player = new PlayerInfo(i, team, role, null);
                players.Add(player);
            }

            return players;
        }

        protected void PlacePlayers(List<PlayerInfo> players, Dictionary<TeamColor, List<Location>> locations)
        {
            var redCounter = 0;
            var blueCounter = 0;
            foreach (var player in players)
                switch (player.Team)
                {
                    case TeamColor.Red:
                        player.Location = locations[TeamColor.Red][redCounter];
                        redCounter++;
                        break;
                    case TeamColor.Blue:
                        player.Location = locations[TeamColor.Blue][blueCounter];
                        blueCounter++;
                        break;
                }

            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                Board.Players.Add(i, player);
                Board[player.Location].PlayerId = i;
            }
        }

        protected List<Location> GenerateLocationsOnRectangle(int count, Location bottomLeftCorner,
            Location topRightCorner)
        {
            var randomLocations = new HashSet<Location>();

            for (var i = 0; i < count; i++)
            {
                Location location;

                do
                {
                    var randomX = _random.Next(bottomLeftCorner.X, topRightCorner.X + 1);
                    var randomY = _random.Next(bottomLeftCorner.Y, topRightCorner.Y + 1);
                    location = new Location(randomX, randomY);
                } while (!randomLocations.Add(location));
            }

            return new List<Location>(randomLocations);
        }
    }
}