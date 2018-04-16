using System.Collections.Generic;
using System.Linq;
using BoardGenerators.Generators;
using Common;
using Common.BoardObjects;
using GameMaster;

namespace TestScenarios.DeterministicGame
{
    public class DeterministicGameMasterBoardGenerator : BoardGeneratorBase<GameMasterBoard>
    {
        protected override GameMasterBoard Board { get; set; }

        public GameMasterBoard InitializeBoard(DeterministicGameDefinition config)
        {
            Board = new GameMasterBoard(config.BoardWidth, config.TaskAreaLength, config.GoalAreaLength);

            PlaceGoals(config.Goals);

            var piecesWithLocations = config.Pieces.Select((x, i) => (new Piece(i, x.Type), x.Location)).ToList();
            PlacePieces(piecesWithLocations);

            var players = config.Players.Select((x, i) => (new PlayerBase(i, x.Team, x.Role), x.Location)).ToList();
            PlacePlayers(players);

            return Board;
        }

        protected override void PlaceGoals(IEnumerable<GoalField> goals)
        {
            base.PlaceGoals(goals);

            foreach (var goal in goals)
            {
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
        }
    }
}
