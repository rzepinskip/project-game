using System.Collections.Generic;
using FluentAssertions;
using TestScenarios.DiscoverScenarios;
using TestScenarios.DiscoverScenarios.DiscoverBoardCorner;
using TestScenarios.DiscoverScenarios.DiscoverGoalAreaCorner;
using TestScenarios.DiscoverScenarios.DiscoverGoalAreaEdge;
using TestScenarios.DiscoverScenarios.DiscoverPiece;
using TestScenarios.DiscoverScenarios.DiscoverPlayer;
using TestScenarios.DiscoverScenarios.DiscoverRegular;
using TestScenarios.DiscoverScenarios.DiscoverTaskAreaBoardEdge;
using TestScenarios.DiscoverScenarios.DiscoverTaskAreaCorner;
using TestScenarios.DiscoverScenarios.DiscoverTaskAreaEdge;
using TestScenarios.DiscoverScenarios.DiscoverUpdate;
using Xunit;

namespace GameMaster.Tests
{
    public class DiscoverTests
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void DiscoverTestsResponse(DiscoverScenarioBase scenario)
        {
            var gameMaster = new GameMaster(scenario.InitialGameMasterBoard, scenario.PlayerGuidToId);

            var response = scenario.InitialRequest.Process(gameMaster);

            response.Should().Be(scenario.Response);
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void DiscoverTestsBoard(DiscoverScenarioBase scenario)
        {
            var gameMaster = new GameMaster(scenario.InitialGameMasterBoard, scenario.PlayerGuidToId);

            scenario.InitialRequest.Process(gameMaster);

            gameMaster.Board.Should().BeEquivalentTo(scenario.UpdatedGameMasterBoard, options => options.Excluding(o => o.Lock));
        }
        public static IEnumerable<object[]> GetData()
        {
            yield return new object[] { new DiscoverRegular() };
            yield return new object[] { new DiscoverTaskAreaEdge() };
            yield return new object[] { new DiscoverTaskAreaBoardEdge() };
            yield return new object[] { new DiscoverTaskAreaCorner() };
            yield return new object[] { new DiscoverGoalAreaCorner() };
            yield return new object[] { new DiscoverGoalAreaEdge() };
            yield return new object[] { new DiscoverBoardCorner() };
            yield return new object[] { new DiscoverPiece() };
            yield return new object[] { new DiscoverPlayer() };
            yield return new object[] { new DiscoverUpdate() };
        }
    }
}
