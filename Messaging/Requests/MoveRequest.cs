using System.Xml.Serialization;
using Common;
using Common.ActionInfo;

namespace Messaging.Requests
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class MoveRequest : Request
    {
        public MoveRequest(string playerGuid, Direction direction) : base(playerGuid)
        {
            Direction = direction;
        }

        [XmlAttribute] public Direction Direction { get; }


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