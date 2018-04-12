using System.Xml.Serialization;
using Common;
using Common.ActionInfo;

namespace Messaging.Requests
{
    [XmlRoot(ElementName = "TestPiece", Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class TestPieceRequest : Request
    {
        public TestPieceRequest(string playerGuid) : base(playerGuid)
        {
        }

        public override string ToLog()
        {
            return string.Join(',', ActionType.Test, base.ToLog());
        }

        public override ActionInfo GetActionInfo()
        {
            return new TestActionInfo(PlayerGuid);
        }
    }
}