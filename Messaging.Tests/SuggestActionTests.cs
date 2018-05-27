using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using FluentAssertions;
using Messaging.Serialization;
using Xunit;

namespace Messaging.Tests
{
    public class SuggestActionTests
    {
        protected ExtendedMessageXmlDeserializer MessageXmlSerializer =
            new ExtendedMessageXmlDeserializer(Constants.DefaultMessagingNameSpace);

        public static IEnumerable<object[]> ActionMessagesTypes =>
            new List<object[]>
            {
                new object[] { "SuggestActionWithGuid" },
                new object[] { "SuggestActionWithoutGuid" },
                new object[] { "SuggestActionResponseWithGuid" },
                new object[] { "SuggestActionResponseWithoutGuid" }
            };

        [Theory]
        [MemberData(nameof(ActionMessagesTypes))]
        public void TestActionMessages(string filename)
        {
            var filePath = $"Resources/SuggestActionMessages/{filename}.xml";
            var expected = File.ReadAllText(filePath);

            var message = MessageXmlSerializer.Deserialize(expected);
            var result = message.SerializeToXml();

            XDocument.Parse(result).Should().BeEquivalentTo(XDocument.Parse(expected));
        }
    }
}
