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
    public class ErrorMessagesTests
    {
        protected ExtendedMessageXmlDeserializer MessageXmlSerializer =
            new ExtendedMessageXmlDeserializer(Constants.DefaultMessagingNameSpace);

        public static IEnumerable<object[]> ErrorMessagesTypes =>
            new List<object[]>
            {
                new object[] {"GameMasterDisconnectedMessage"},
                new object[] {"PlayerDisconnectedMessage"},
            };

        [Theory]
        [MemberData(nameof(ErrorMessagesTypes))]
        public void TestErrorMessages(string filename)
        {
            var filePath = $"Resources/ErrorMessages/{filename}.xml";
            var expected = File.ReadAllText(filePath);

            var message = MessageXmlSerializer.Deserialize(expected);
            var result = message.SerializeToXml();

            XDocument.Parse(result).Should().BeEquivalentTo(XDocument.Parse(expected));
        }
    }
}
