using System.Xml.Serialization;
using Common;
using Common.ActionInfo;

namespace Messaging.Requests
{
    [XmlType(XmlRootName)]
    public class PickUpPieceRequest : Request
    {
        public const string XmlRootName = "PickUpPiece";

        protected PickUpPieceRequest()
        {
        }

        public PickUpPieceRequest(string playerGuid) : base(playerGuid)
        {
        }

        public override ActionInfo GetActionInfo()
        {
            return new PickUpActionInfo(PlayerGuid);
        }

        public override string Serialize()
        {
            return XmlExtensions.SerializeToXml(this);
        }

        public override string ToLog()
        {
            return string.Join(',', ActionType.PickUp, base.ToLog());
        }
    }
}