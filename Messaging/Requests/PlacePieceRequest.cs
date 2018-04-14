using System.Xml.Serialization;
using Common;
using Common.ActionInfo;

namespace Messaging.Requests
{
    [XmlType(XmlRootName)]
    public class PlacePieceRequest : Request
    {
        public const string XmlRootName = "PlacePiece";

        protected PlacePieceRequest()
        {
        }

        public PlacePieceRequest(string playerGuid) : base(playerGuid)
        {
        }

        public override ActionInfo GetActionInfo()
        {
            return new PlaceActionInfo(PlayerGuid);
        }

        public override string Serialize()
        {
            return XmlExtensions.SerializeToXml(this);
        }

        public override string ToLog()
        {
            return string.Join(',', ActionType.Place, base.ToLog());
        }
    }
}