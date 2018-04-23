using FluentAssertions;
using TestScenarios;
using Xunit;

namespace Player.Tests
{
    public class ActionsTests
    {
        [Theory]
        [ClassData (typeof(TestsDataset))]
        public void TestActionBoardsUpdate(ScenarioBase scenario)
        {
            var player = new Player();
            var playerInfo = scenario.InitialPlayerBoard.Players[scenario.PlayerId];
            player.InitializePlayer(scenario.PlayerId, scenario.PlayerGuid, playerInfo.Team, playerInfo.Role,
                scenario.InitialPlayerBoard, playerInfo.Location);

            scenario.Response.Process(player);
            
            player.Board.Should().BeEquivalentTo(scenario.UpdatedPlayerBoard);
            player.Board.Should().Be(scenario.UpdatedPlayerBoard);
        }
    }
}