using System.Xml.Serialization;
using Common;
using Common.ActionInfo;

namespace Messaging.Requests
{
    [XmlRoot(ElementName = "Discover", Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class DiscoverRequest : Request
    {
        protected DiscoverRequest()
        {
        }

        public DiscoverRequest(string playerGuid) : base(playerGuid)
        {
        }

        public override string ToLog()
        {
            return string.Join(',', ActionType.Discover, base.ToLog());
        }

        public override ActionInfo GetActionInfo()
        {
            return new DiscoverActionInfo(PlayerGuid);
        }
    }
}