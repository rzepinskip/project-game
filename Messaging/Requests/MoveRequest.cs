using System.Xml.Serialization;
using Common;
using Common.ActionInfo;
using System;
using System.Collections.Generic;

namespace Messaging.Requests
{
    [XmlType(XmlRootName)]
    public class MoveRequest : Request, IEquatable<MoveRequest>
    {
        public const string XmlRootName = "Move";

        protected MoveRequest()
        {
        }

        public MoveRequest(Guid playerGuid, int gameId, Direction direction) : base(playerGuid, gameId)
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

        public override bool Equals(object obj)
        {
            return Equals(obj as MoveRequest);
        }

        public bool Equals(MoveRequest other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Direction == other.Direction;
        }

        public override int GetHashCode()
        {
            var hashCode = 1644100618;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Direction.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(MoveRequest request1, MoveRequest request2)
        {
            return EqualityComparer<MoveRequest>.Default.Equals(request1, request2);
        }

        public static bool operator !=(MoveRequest request1, MoveRequest request2)
        {
            return !(request1 == request2);
        }
    }
}