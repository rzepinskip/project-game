using System.Xml.Serialization;
using Common;
using Common.ActionInfo;

namespace Messaging.Requests
{
    [XmlRoot(ElementName = "PickUpPiece", Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class PickUpPieceRequest : Request
    {
        public PickUpPieceRequest(string playerGuid) : base(playerGuid)
        {
        }

        public override string ToLog()
        {
            return string.Join(',', ActionType.PickUp, base.ToLog());
        }

        public override ActionInfo GetActionInfo()
        {
            return new PickUpActionInfo(PlayerGuid);
        }
    }
}