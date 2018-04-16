using System.Xml.Serialization;
using Common;
using Common.ActionInfo;

namespace Messaging.Requests
{
    [XmlType(XmlRootName)]
    public class MoveRequest : Request
    {
        public const string XmlRootName = "Move";

        protected MoveRequest()
        {
        }

        public MoveRequest(string playerGuid, int gameId, Direction direction) : base(playerGuid, gameId)
        {
            Direction = direction;
        }

        [XmlAttribute("direction")] public Direction Direction { get; set; }

        public override ActionInfo GetActionInfo()
        {
            return new MoveActionInfo(PlayerGuid, Direction);
        }

        public override string ToLog()
        {
            return string.Join(',', ActionType.Move, base.ToLog());
        }
    }
}