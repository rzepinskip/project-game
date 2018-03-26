namespace Common.ActionInfo
{
    public class MoveActionInfo : ActionInfo
    {
        public MoveActionInfo(string playerGuid, Direction direction) : base(playerGuid)
        {
            Direction = direction;
        }

        public Direction Direction { get; }
    }
}