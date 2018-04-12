using System.Xml.Serialization;
using Common;
using Common.ActionInfo;

namespace Messaging.Requests
{
    [XmlInclude(typeof(MoveRequest))]
    [XmlRoot(ElementName = "Move", Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class MoveRequest : Request
    {
        protected MoveRequest()
        {
        }

        public MoveRequest(string playerGuid, Direction direction) : base(playerGuid)
        {
            Direction = direction;
        }

        [XmlAttribute("direction")] public Direction Direction { get; set; }


        public override string ToLog()
        {
            return string.Join(',', ActionType.Move, base.ToLog());
        }

        public override ActionInfo GetActionInfo()
        {
            return new MoveActionInfo(PlayerGuid, Direction);
        }
    }
}