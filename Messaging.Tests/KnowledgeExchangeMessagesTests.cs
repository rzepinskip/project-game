using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using FluentAssertions;
using Messaging.InitializationMessages;
using Messaging.KnowledgeExchangeMessages;
using Messaging.Serialization;
using Xunit;

namespace Messaging.Tests
{
    public class KnowledgeExchangeMessagesTests
    {
        private const string DefaultNamespace = "https://se2.mini.pw.edu.pl/17-results/";

        protected ExtendedMessageXmlDeserializer MessageXmlSerializer =
            new ExtendedMessageXmlDeserializer(DefaultNamespace);

        public static IEnumerable<object[]> InitializationMessagesTypes =>
            new List<object[]>
            {
                new object[] {typeof(KnowledgeExchangeRequestMessage) },
                new object[] {typeof(RejectKnowledgeExchangeMessage) },
            };

        [Theory]
        [MemberData(nameof(InitializationMessagesTypes))]
        public void TestKnowledgeExchangeMessages(Type messageType)
        {
            var filePath = $"Resources/KnowledgeExchangeMessages/{messageType.Name}.xml";
            var expected = File.ReadAllText(filePath);

            var message = MessageXmlSerializer.Deserialize(expected);
            var result = message.SerializeToXml();

            XDocument.Parse(result).Should().BeEquivalentTo(XDocument.Parse(expected));
        }
    }
}