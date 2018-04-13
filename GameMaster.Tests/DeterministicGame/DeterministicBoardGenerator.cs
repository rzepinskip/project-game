using System.Collections.Generic;
using System.Linq;
using BoardGenerators.Generators;
using Common;
using Common.BoardObjects;
using GameMaster.Tests.BoardConfigurationGenerator;

namespace GameMaster.Tests.DeterministicGame
{
    /*
    public class DeterministicBoardGenerator : BoardGeneratorBase
    {
        public GameMasterBoard InitializeBoard(DeterministicGameDefinition config)
        {
            var board = new GameMasterBoard(config.BoardWidth, config.TaskAreaLength, config.GoalAreaLength);
            Board = board;

            PlaceGoals(config.Goals);

            var piecesWithLocations = config.Pieces.Select((x, i) => (new Piece(i, x.Type), x.Location)).ToList();
            PlacePieces(piecesWithLocations);

            var players = config.Players.Select((x, i) => (new PlayerBase(i, x.Team, x.Role), x.Location)).ToList();
            PlacePlayers(players);

            return board;
        }
    }
    */
}
