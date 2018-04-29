using System.Xml.Linq;
using FluentAssertions;
using Messaging.Serialization;
using TestScenarios;
using Xunit;

namespace Messaging.Tests
{
    public class ActionsTests
    {
        private const string DefaultNamespace = "https://se2.mini.pw.edu.pl/17-results/";

        protected ExtendedMessageXmlDeserializer MessageXmlSerializer =
            new ExtendedMessageXmlDeserializer(DefaultNamespace);

        [Theory]
        [ClassData(typeof(TestsDataset))]
        public void TestRequests(ScenarioBase scenario)
        {
            var xml = MessageXmlSerializer.SerializeToXml(scenario.InitialRequest);

            XDocument.Parse(xml).Should().BeEquivalentTo(XDocument.Parse(scenario.InitialRequestFileContent));
        }

        [Theory]
        [ClassData(typeof(TestsDataset))]
        public void TestResponses(ScenarioBase scenario)
        {
            var xml = MessageXmlSerializer.SerializeToXml(scenario.Response);

            XDocument.Parse(xml).Should().BeEquivalentTo(XDocument.Parse(scenario.ResponseFileContent));
        }
    }
}