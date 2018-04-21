using System.Collections.Generic;
using TestScenarios.MoveScenarios;
using TestScenarios.MoveScenarios.MoveToGoalField;
using TestScenarios.MoveScenarios.MoveToTasFieldWithoutPiece;
using TestScenarios.MoveScenarios.MoveToTaskFieldOccupiedByPlayerWhoDoesntCarryPiece;
using TestScenarios.MoveScenarios.MoveToTaskFieldWithPiece;
using TestScenarios.MoveScenarios.MoveToTaskFieldWithPieceOccupiedByPlayerWhoDoesntCarryPiece;
using Xunit;

namespace Messaging.Tests
{
    public class MoveTests : MessagingTestsBase
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void MoveTestsRequest(MoveScenarioBase scenario)
        {
            var xml = MessageXmlSerializer.SerializeToXml(scenario.InitialRequest);

            Assert.False(XmlsHasDiffrences(scenario.InitialRequestFileContent, xml));
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void MoveTestsResponse(MoveScenarioBase scenario)
        {
            var xml = MessageXmlSerializer.SerializeToXml(scenario.Response);

            Assert.False(XmlsHasDiffrences(scenario.ResponseFileContent, xml));
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[] { new MoveToGoalField() };
            yield return new object[] { new MoveToTaskFieldWithoutPiece() };
            yield return new object[] { new MoveToTaskFieldWithPiece() };
            yield return new object[] { new MoveToTaskFieldWithoutPiece() };
            //yield return new object[] { new MoveToTaskFieldOccupiedByPlayerWhoCarryPiece()};
            yield return new object[] { new MoveToTaskFieldOccupiedByPlayerWhoDoesntCarryPiece() };
            //yield return new object[] { new MoveToTaskFieldWithPieceOccupiedByPlayerWhoCarryPiece()};
            yield return new object[] { new MoveToTaskFieldWithPieceOccupiedByPlayerWhoDoesntCarryPiece() };
        }

    }
}
