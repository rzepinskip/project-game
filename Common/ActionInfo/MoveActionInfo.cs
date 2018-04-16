using System;

namespace Common.ActionInfo
{
    public class MoveActionInfo : ActionInfo
    {
        public MoveActionInfo(Guid playerGuid, Direction direction) : base(playerGuid)
        {
            Direction = direction;
        }

        public Direction Direction { get; }
    }
}