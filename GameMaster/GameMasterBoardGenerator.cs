using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BoardGenerators.Generators;
using Common;
using Common.BoardObjects;
using GameMaster.Configuration;

namespace GameMaster
{
    public class GameMasterBoardGenerator : BoardGeneratorBase<GameMasterBoard>
    {
        private readonly Random _random = new Random();

        protected override GameMasterBoard Board { get; set; }

        public List<(TeamColor, PlayerType)> GeneratePlayerSlots(int teamPlayerCount)
        {
            var playersCount = 2 * teamPlayerCount;
            var playersSlots = new List<(TeamColor, PlayerType)>(playersCount);

            for (var i = 0; i < playersCount; i++)
            {
                var team = i % 2 == 0 ? TeamColor.Red : TeamColor.Blue;
                var role = PlayerType.Member;

                if (i < 2)
                    role = PlayerType.Leader;

                var player = (team, role);
                playersSlots.Add(player);
            }

            return playersSlots;
        }

        public GameMasterBoard InitializeBoard(GameDefinition gameDefinition)
        {
            Board = new GameMasterBoard(gameDefinition.BoardWidth, gameDefinition.TaskAreaLength,
                gameDefinition.GoalAreaLength);

            return Board;
        }

        public void SpawnGameObjects(GameDefinition gameDefinition)
        {
            var pieces = GeneratePieces(gameDefinition.InitialNumberOfPieces, gameDefinition.ShamProbability);
            var piecesLocations = GenerateLocationsForPieces(gameDefinition.InitialNumberOfPieces);
            var piecesWithLocation = pieces.Zip(piecesLocations, (p, l) => (p, l));
            PlacePieces(piecesWithLocation);

            PlaceGoals(gameDefinition.Goals);

            var playersLocations = GenerateLocationsForPlayers(gameDefinition.NumberOfPlayersPerTeam);
            var playersWithLocation = AssignLocationsToPlayers(Board.Players.Values, playersLocations);
            PlacePlayers(playersWithLocation);
        }

        protected void PlacePieces(IEnumerable<(Piece piece, Location location)> piecesWithLocations)
        {
            Board.Pieces.Clear();
            foreach (var (piece, location) in piecesWithLocations)
            {
                var taskFieldToFill = Board[location] as TaskField;
                if (taskFieldToFill == null)
                    throw new InvalidDataException();

                taskFieldToFill.DistanceToPiece = 0;
                taskFieldToFill.PieceId = piece.Id;

                Board.Pieces.Add(piece.Id, piece);
            }
        }

        protected override void PlaceGoals(IEnumerable<GoalField> goals)
        {
            var goalFields = goals.ToList();

            base.PlaceGoals(goalFields);

            foreach (var goal in goalFields)
                switch (goal.Team)
                {
                    case TeamColor.Blue:
                        Board.UncompletedBlueGoalsLocations.Add(goal);
                        break;
                    case TeamColor.Red:
                        Board.UncompletedRedGoalsLocations.Add(goal);
                        break;
                }
        }

        private IEnumerable<(PlayerBase player, Location location)> AssignLocationsToPlayers(
            IEnumerable<PlayerBase> players, Dictionary<TeamColor, Stack<Location>> playersLocations)
        {
            var playersWithLocation = new List<(PlayerBase player, Location location)>();

            foreach (var player in players)
            {
                var location = playersLocations[player.Team].Pop();
                playersWithLocation.Add((player, location));
            }

            return playersWithLocation;
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

        private IEnumerable<Location> GenerateLocationsForPieces(int count)
        {
            var taskAreaBottomLeftCorner = new Location(0, Board.GoalAreaSize);
            var taskAreaTopRightCorner = new Location(Board.Width - 1, Board.Height - (Board.GoalAreaSize + 1));
            var locations = GenerateLocationsOnRectangle(count, taskAreaBottomLeftCorner, taskAreaTopRightCorner);

            return locations;
        }

        private Dictionary<TeamColor, Stack<Location>> GenerateLocationsForPlayers(int teamPlayerCount)
        {
            var redBottomLeftCorner = new Location(0, Board.Height - Board.GoalAreaSize);
            var redTopRightCorner = new Location(Board.Width - 1, Board.Height - 1);
            var redTeamLocations =
                new Stack<Location>(GenerateLocationsOnRectangle(teamPlayerCount, redBottomLeftCorner,
                    redTopRightCorner));

            var blueBottomLeftCorner = new Location(0, 0);
            var blueTopRightCorner = new Location(Board.Width - 1, Board.GoalAreaSize - 1);
            var blueTeamLocations =
                new Stack<Location>(GenerateLocationsOnRectangle(teamPlayerCount, blueBottomLeftCorner,
                    blueTopRightCorner));

            var playersLocations = new Dictionary<TeamColor, Stack<Location>>
            {
                {TeamColor.Blue, blueTeamLocations},
                {TeamColor.Red, redTeamLocations}
            };

            return playersLocations;
        }

        private List<Location> GenerateLocationsOnRectangle(int count, Location bottomLeftCorner,
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