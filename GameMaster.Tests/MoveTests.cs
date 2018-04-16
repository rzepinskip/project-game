﻿using System.Collections.Generic;
using TestScenarios.MoveScenarios;
using TestScenarios.MoveScenarios.MoveToGoalField;
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

            Assert.Equal(scenario.Response, response);
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void MoveTestsBoard(MoveScenarioBase scenario)
        {
            var gameMaster = new GameMaster(scenario.InitialGameMasterBoard, scenario.PlayerGuidToId);

            scenario.InitialRequest.Process(gameMaster);

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