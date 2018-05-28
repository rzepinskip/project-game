using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using FluentAssertions;
using Messaging.InitializationMessages;
using Messaging.Serialization;
using Xunit;

namespace Messaging.Tests
{
    public class InitializationMessagesTests
    {
        protected ExtendedMessageXmlDeserializer MessageXmlSerializer =
            new ExtendedMessageXmlDeserializer(Constants.DefaultMessagingNameSpace);

        public static IEnumerable<object[]> InitializationMessagesTypes =>
            new List<object[]>
            {
                new object[] {"GetGamesMessage"},
                new object[] {"JoinGameMessage"},
                new object[] {"RegisteredGamesMessage"},
                new object[] {"ConfirmGameRegistrationMessage"},
                new object[] {"ConfirmJoiningGameMessage" },
                new object[] {"GameMessage"},
                new object[] {"RejectGameRegistrationMessage"},
                new object[] {"RejectJoiningGame"},
                new object[] {"RegisterGame"}
            };

        [Theory]
        [MemberData(nameof(InitializationMessagesTypes))]
        public void TestInitializationMessages(string filename)
        {
            var filePath = $"Resources/InitializationMessages/{filename}.xml";
            var expected = File.ReadAllText(filePath);

            var message = MessageXmlSerializer.Deserialize(expected);
            var result = message.SerializeToXml();

            XDocument.Parse(result).Should().BeEquivalentTo(XDocument.Parse(expected));
        }
    }
}