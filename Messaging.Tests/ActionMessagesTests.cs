using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using FluentAssertions;
using Messaging.ActionsMessages;
using Messaging.InitializationMessages;
using Messaging.Requests;
using Messaging.Serialization;
using Xunit;

namespace Messaging.Tests
{
    public class ActionMessagesTests
    {
        private const string DefaultNamespace = "https://se2.mini.pw.edu.pl/17-results/";

        protected ExtendedMessageXmlDeserializer MessageXmlSerializer =
            new ExtendedMessageXmlDeserializer(DefaultNamespace);

        public static IEnumerable<object[]> ActionMessagesTypes =>
            new List<object[]>
            {
                new object[] {typeof(AuthorizeKnowledgeExchangeRequest) },
                new object[] {typeof(DestroyPieceRequest) },
                new object[] {typeof(DiscoverRequest) },
                new object[] {typeof(MoveRequest) },
                new object[] {typeof(PickUpPieceRequest) },
                new object[] {typeof(PlacePieceRequest) },
                new object[] {typeof(TestPieceRequest) },
                new object[] {typeof(DataMessage) },
            };

        [Theory]
        [MemberData(nameof(ActionMessagesTypes))]
        public void TestActionMessages(Type messageType)
        {
            var filePath = $"Resources/ActionMessages/{messageType.Name}.xml";
            var expected = File.ReadAllText(filePath);

            var message = MessageXmlSerializer.Deserialize(expected);
            var result = message.SerializeToXml();

            XDocument.Parse(result).Should().BeEquivalentTo(XDocument.Parse(expected));
        }
    }
}
