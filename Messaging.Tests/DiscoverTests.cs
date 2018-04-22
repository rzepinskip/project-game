using System.Collections.Generic;
using Messaging.Serialization;
using Org.XmlUnit.Builder;
using TestScenarios.DiscoverScenarios;
using TestScenarios.DiscoverScenarios.DiscoverBoardCorner;
using TestScenarios.DiscoverScenarios.DiscoverGoalAreaCorner;
using TestScenarios.DiscoverScenarios.DiscoverGoalAreaEdge;
using TestScenarios.DiscoverScenarios.DiscoverPiece;
using TestScenarios.DiscoverScenarios.DiscoverRegular;
using TestScenarios.DiscoverScenarios.DiscoverTaskAreaBoardEdge;
using TestScenarios.DiscoverScenarios.DiscoverTaskAreaCorner;
using TestScenarios.DiscoverScenarios.DiscoverTaskAreaEdge;
using Xunit;

namespace Messaging.Tests
{
    public class DiscoverTests : MessagingTestsBase
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void DiscoverTestsRequest(DiscoverScenarioBase scenario)
        {
            var xml = MessageXmlSerializer.SerializeToXml(scenario.InitialRequest);

            Assert.False(XmlsHasDiffrences(scenario.InitialRequestFileContent, xml));
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void DiscoverTestsResponse(DiscoverScenarioBase scenario)
        {
            var xml = MessageXmlSerializer.SerializeToXml(scenario.Response);

            Assert.False(XmlsHasDiffrences(scenario.ResponseFileContent, xml));
        }

        public static IEnumerable<object[]> GetData()
        {
            //yield return new object[] { new DiscoverRegular() };
            //yield return new object[] { new DiscoverTaskAreaEdge() };
            //yield return new object[] { new DiscoverTaskAreaBoardEdge() };
            //yield return new object[] { new DiscoverTaskAreaCorner() };
            //yield return new object[] { new DiscoverGoalAreaCorner() };
            //yield return new object[] { new DiscoverGoalAreaEdge() };
            //yield return new object[] { new DiscoverBoardCorner() };
            yield return new object[] { new DiscoverPiece() };
            //yield return new object[] { new DiscoverPlayer() };
            //yield return new object[] { new DiscoverUpdate() };  }
        }
    }
}
