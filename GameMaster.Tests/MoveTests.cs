using System.Collections.Generic;
using FluentAssertions;
using TestScenarios.MoveScenarios;
using TestScenarios.MoveScenarios.MoveToGoalField;
using TestScenarios.MoveScenarios.MoveToTasFieldWithoutPiece;
using TestScenarios.MoveScenarios.MoveToTaskFieldWithPiece;
using TestScenarios.MoveScenarios.MoveToTaskFieldOccupiedByPlayerWhoDoesntCarryPiece;
using TestScenarios.MoveScenarios.MoveToTaskFieldWithPieceOccupiedByPlayerWhoDoesntCarryPiece;
using Xunit;

namespace GameMaster.Tests
{
    public class MoveTests
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void MoveTestsResponse(MoveScenarioBase scenario)
        {
            var gameMaster = new GameMaster(scenario.InitialGameMasterBoard, scenario.PlayerGuidToId);

            var response = scenario.InitialRequest.Process(gameMaster);

            response.Should().Be(scenario.Response);
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void MoveTestsBoard(MoveScenarioBase scenario)
        {
            var gameMaster = new GameMaster(scenario.InitialGameMasterBoard, scenario.PlayerGuidToId);

            scenario.InitialRequest.Process(gameMaster);

            gameMaster.Board.Should().BeEquivalentTo(scenario.UpdatedGameMasterBoard, options => options.Excluding(o => o.Lock));
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[] { new MoveToGoalField() };
            yield return new object[] { new MoveToTaskFieldWithoutPiece() };
            yield return new object[] { new MoveToTaskFieldWithPiece() };
            //yield return new object[] { new MoveToTaskFieldOccupiedByPlayerWhoCarryPiece()};
            yield return new object[] { new MoveToTaskFieldOccupiedByPlayerWhoDoesntCarryPiece() };
            //yield return new object[] { new MoveToTaskFieldWithPieceOccupiedByPlayerWhoCarryPiece()};
            yield return new object[] { new MoveToTaskFieldWithPieceOccupiedByPlayerWhoDoesntCarryPiece() };
        }

    }
}
