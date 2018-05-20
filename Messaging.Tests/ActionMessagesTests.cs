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
        protected ExtendedMessageXmlDeserializer MessageXmlSerializer =
            new ExtendedMessageXmlDeserializer(Constants.DefaultMessagingNameSpace);

        public static IEnumerable<object[]> ActionMessagesTypes =>
            new List<object[]>
            {
                new object[] {"AuthorizeKnowledgeExchangeRequest"},
                new object[] {"DestroyPieceRequest"},
                new object[] {"DiscoverRequest"},
                new object[] {"MoveRequest"},
                new object[] {"PickUpPieceRequest"},
                new object[] {"PlacePieceRequest"},
                new object[] {"TestPieceRequest"},
                new object[] {"DataMessageWithGuid"},
                new object[] {"DataMessageWithoutGuid"},
            };

        [Theory]
        [MemberData(nameof(ActionMessagesTypes))]
        public void TestActionMessages(string filename)
        {
            var filePath = $"Resources/ActionMessages/{filename}.xml";
            var expected = File.ReadAllText(filePath);

            var message = MessageXmlSerializer.Deserialize(expected);
            var result = message.SerializeToXml();

            XDocument.Parse(result).Should().BeEquivalentTo(XDocument.Parse(expected));
        }
    }
}
