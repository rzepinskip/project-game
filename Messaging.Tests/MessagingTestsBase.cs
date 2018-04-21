using Messaging.Serialization;
using Org.XmlUnit.Builder;

namespace Messaging.Tests
{
    public abstract class MessagingTestsBase
    {
        private const string DefaultNamespace = "https://se2.mini.pw.edu.pl/17-results/";
        protected ExtendedMessageXmlDeserializer MessageXmlSerializer = new ExtendedMessageXmlDeserializer(DefaultNamespace);

        protected bool XmlsHasDiffrences(string expected, string actual)
        {
            var d = DiffBuilder.Compare(Input.FromString(expected)).WithTest(actual).Build();
            return d.HasDifferences();
        }
    }
}
