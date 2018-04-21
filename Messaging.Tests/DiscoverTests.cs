using System;
using System.Collections.Generic;
using System.Text;
using Messaging.Serialization;
using Org.XmlUnit.Builder;
using Org.XmlUnit.Diff;
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

namespace Messaging.Tests
{
    public class DiscoverTests
    {
        private const string DefaultNamespace = "https://se2.mini.pw.edu.pl/17-results/";

        private ExtendedMessageXmlDeserializer _messageXmlSerializer =
            new ExtendedMessageXmlDeserializer(DefaultNamespace);

        private bool XmlsHasDiffrences(string expected, string actual)
        {
            var d = DiffBuilder.Compare(Input.FromString(expected)).WithTest(actual).Build();
            return d.HasDifferences();
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void DiscoverTestsRequest(DiscoverScenarioBase scenario)
        {
            var xml = _messageXmlSerializer.SerializeToXml(scenario.InitialRequest);

            Assert.False(XmlsHasDiffrences(scenario.InitialRequestFileContent, xml));
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void DiscoverTestsResponse(DiscoverScenarioBase scenario)
        {
            var xml = _messageXmlSerializer.SerializeToXml(scenario.Response);

            Assert.False(XmlsHasDiffrences(scenario.ResponseFileContent, xml));
        }

        public static IEnumerable<object[]> GetData()
        {
            //yield return new object[] { new DiscoverRegular() };
            //yield return new object[] { new DiscoverTaskAreaEdge() };
            //yield return new object[] { new DiscoverTaskAreaBoardEdge() };
            //yield return new object[] { new DiscoverTaskAreaCorner() };
            //yield return new object[] { new DiscoverGoalAreaCorner() };
            yield return new object[] { new DiscoverGoalAreaEdge() };
            //yield return new object[] { new DiscoverBoardCorner() };
            //yield return new object[] { new DiscoverPiece() };
            //yield return new object[] { new DiscoverPlayer() };
            //yield return new object[] { new DiscoverUpdate() };  }
        }
    }
}
