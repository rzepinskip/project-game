using System.Linq;
using BoardGenerators.Generators;
using Common;
using Player;

namespace TestScenarios.DeterministicGame
{
    public class DeterministicPlayerBoardGenerator : BoardGeneratorBase<PlayerBoard>
    {
        protected override PlayerBoard Board { get; set; }

        public PlayerBoard InitializeBoard(DeterministicGameDefinition config, int playerId)
        {
            Board = new PlayerBoard(config.BoardWidth, config.TaskAreaLength, config.GoalAreaLength);

            var players = config.Players.Where(p => p.Id == playerId)
                .Select((x, i) => (new PlayerBase(i, x.Team, x.Role), x.Location)).ToList();
            PlacePlayers(players);

            return Board;
        }
    }
}