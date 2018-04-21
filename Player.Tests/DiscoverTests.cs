using System.Collections.Generic;
using TestScenarios.DiscoverScenarios;
using TestScenarios.DiscoverScenarios.DiscoverGoalAreaEdge;
using TestScenarios.DiscoverScenarios.DiscoverRegular;
using TestScenarios.DiscoverScenarios.DiscoverTaskAreaBoardEdge;
using TestScenarios.DiscoverScenarios.DiscoverTaskAreaCorner;
using TestScenarios.DiscoverScenarios.DiscoverTaskAreaEdge;
using TestScenarios.MoveScenarios;
using TestScenarios.MoveScenarios.MoveToGoalField;
using TestScenarios.MoveScenarios.MoveToTasFieldWithoutPiece;
using TestScenarios.MoveScenarios.MoveToTaskFieldOccupiedByPlayerWhoDoesntCarryPiece;
using TestScenarios.MoveScenarios.MoveToTaskFieldWithPiece;
using TestScenarios.MoveScenarios.MoveToTaskFieldWithPieceOccupiedByPlayerWhoDoesntCarryPiece;
using Xunit;

namespace Player.Tests
{
    public class DiscoverTests
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void DiscoverTestsBoard(DiscoverScenarioBase scenario)
        {
            var player = new Player();
            var playerInfo = scenario.InitialPlayerBoard.Players[scenario.PlayerId];
            player.InitializePlayer(scenario.PlayerId, scenario.PlayerGuid, playerInfo.Team, playerInfo.Role,
                scenario.InitialPlayerBoard, playerInfo.Location);

            scenario.Response.Process(player);

            Assert.Equal(scenario.UpdatedPlayerBoard, player.Board);
        }


        public static IEnumerable<object[]> GetData()
        {
            //yield return new object[] { new DiscoverRegular() };
            //yield return new object[] { new DiscoverTaskAreaEdge() };
            yield return new object[] { new DiscoverTaskAreaBoardEdge() };
            //yield return new object[] { new DiscoverTaskAreaCorner() };
            //yield return new object[] { new DiscoverGoalAreaCorner() };
            //yield return new object[] { new DiscoverGoalAreaEdge() };
            //yield return new object[] { new DiscoverBoardCorner() };
            //yield return new object[] { new DiscoverPiece() };
            //yield return new object[] { new DiscoverPlayer() };
            //yield return new object[] { new DiscoverUpdate() };
        }
    }
}