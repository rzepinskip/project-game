using System.Xml.Serialization;
using Common;
using Common.ActionInfo;

namespace Messaging.Requests
{
    [XmlType(XmlRootName)]
    public class DiscoverRequest : Request
    {
        public const string XmlRootName = "Discover";

        protected DiscoverRequest()
        {
        }

        public DiscoverRequest(string playerGuid) : base(playerGuid)
        {
        }

        public override ActionInfo GetActionInfo()
        {
            return new DiscoverActionInfo(PlayerGuid);
        }

        public override string ToLog()
        {
            return string.Join(',', ActionType.Discover, base.ToLog());
        }
    }
}