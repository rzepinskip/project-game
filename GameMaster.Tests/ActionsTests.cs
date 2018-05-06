using FluentAssertions;
using TestScenarios;
using Xunit;

namespace GameMaster.Tests
{
    public class ActionsTests
    {
        [Theory]
        [ClassData(typeof(TestsDataset))]
        public void TestActionResponses(ScenarioBase scenario)
        {
            var gameMaster = new GameMaster(scenario.InitialGameMasterBoard, scenario.PlayerGuidToId);

            var response = scenario.InitialRequest.Process(gameMaster);

            response.Should().Be(scenario.Response);
        }

        [Theory]
        [ClassData(typeof(TestsDataset))]
        public void TestActionBoardsUpdate(ScenarioBase scenario)
        {
            var gameMaster = new GameMaster(scenario.InitialGameMasterBoard, scenario.PlayerGuidToId);

            scenario.InitialRequest.Process(gameMaster);

            gameMaster.Board.Should()
                .BeEquivalentTo(scenario.UpdatedGameMasterBoard, options => options.Excluding(o => o.Lock));
            gameMaster.Board.Should().Be(scenario.UpdatedGameMasterBoard);
        }
    }
}