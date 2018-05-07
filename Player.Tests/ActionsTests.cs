using FluentAssertions;
using TestScenarios;
using Xunit;

namespace Player.Tests
{
    public class ActionsTests
    {
        [Theory]
        [ClassData(typeof(TestsDataset))]
        public void TestActionBoardsUpdate(ScenarioBase scenario)
        {
            var playerInfo = scenario.InitialPlayerBoard.Players[scenario.PlayerId];
            var player = new Player(scenario.PlayerId, scenario.PlayerGuid, playerInfo.Team, playerInfo.Role,
                scenario.InitialPlayerBoard, playerInfo.Location);
            player.PlayerBoard.Players[scenario.PlayerId] = playerInfo;

            scenario.Response.Process(player);

            player.Board.Should().BeEquivalentTo(scenario.UpdatedPlayerBoard);
            player.Board.Should().Be(scenario.UpdatedPlayerBoard);
        }
    }
}