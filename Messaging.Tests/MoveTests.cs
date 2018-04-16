using System;
using System.Collections.Generic;
using System.Text;
using Messaging.Serialization;
using Org.XmlUnit.Builder;
using Org.XmlUnit.Diff;
using TestScenarios.MoveScenarios;
using TestScenarios.MoveScenarios.MoveToGoalField;
using Xunit;

namespace Messaging.Tests
{
    public class MoveTests
    {
        private const string DefaultNamespace = "https://se2.mini.pw.edu.pl/17-results/";
        private ExtendedMessageXmlDeserializer _messageXmlSerializer = new ExtendedMessageXmlDeserializer(DefaultNamespace);

        private bool XmlsHasDiffrences(string expected, string actual)
        {
            var d = DiffBuilder.Compare(Input.FromString(expected)).WithTest(actual).Build();
            return d.HasDifferences();
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void MoveTestsRequest(MoveScenarioBase scenario)
        {
            var xml = _messageXmlSerializer.SerializeToXml(scenario.InitialRequest);

            Assert.False(XmlsHasDiffrences(scenario.InitialRequestFileContent, xml));
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void MoveTestsResponse(MoveScenarioBase scenario)
        {
            var xml = _messageXmlSerializer.SerializeToXml(scenario.Response);

            Assert.False(XmlsHasDiffrences(scenario.ResponseFileContent, xml));
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
