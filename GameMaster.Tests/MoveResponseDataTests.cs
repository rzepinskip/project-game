using System.Collections.Generic;
using Common.Interfaces;
using GameMaster.Configuration;
using TestScenarios.MoveScenarios;
using Xunit;

namespace GameMaster.Tests
{
    public class MoveResponseDataTests
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void MoveTestVoid(MoveScenarioBase scenario)
        {
            var gameMaster = new GameMaster(scenario.InitGameMasterBoard, scenario.PlayerGuidToId);

            var response = scenario.InitialRequest.Process(gameMaster);

            Assert.Equal(scenario.Response, response);
            Assert.Equal(scenario.UpdatedGameMasterBoard, gameMaster.Board);
        }


        public static IEnumerable<object[]> GetData()
        {
            yield return new object[] {new MoveToGoalField()};
            //yield return new object[] { new MoveToTaskField()};
            //yield return new object[] { new MoveToTaskFieldWithPiece()};
            //yield return new object[] { new MoveToTaskFieldWithoutPiece()};
            //yield return new object[] { new MoveToTaskFieldOccupiedByPlayerWhoCarryPiece()};
            //yield return new object[] { new MoveToTaskFieldOccupiedByPlayerWhoDoesntCarryPiece()};
            //yield return new object[] { new MoveToTaskFieldWithPieceOccupiedByPlayerWhoCarryPiece()};
            //yield return new object[] { new MoveToTaskFieldWithPieceOccupiedByPlayerWhoDoesntCarryPiece()};
        }

    }
}
