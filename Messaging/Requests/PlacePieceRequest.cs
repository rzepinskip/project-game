using System.Xml.Serialization;
using Common;
using Common.ActionInfo;

namespace Messaging.Requests
{
    [XmlRoot(ElementName = "PlacePiece", Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class PlacePieceRequest : Request
    {
        public PlacePieceRequest(string playerGuid) : base(playerGuid)
        {
        }

        public override ActionInfo GetActionInfo()
        {
            return new PlaceActionInfo(PlayerGuid);
        }

        public override string ToLog()
        {
            return string.Join(',', ActionType.Place, base.ToLog());
        }
    }
}