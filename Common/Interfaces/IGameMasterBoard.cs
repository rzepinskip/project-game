namespace Common.Interfaces
{
    public interface IGameMasterBoard : IBoard
    {
        IMessage Discover(string playerGuid);
        IMessage Move(string playerGuid, Direction direction);
        IMessage PickUpPiece(string playerGuid);
        IMessage PlacePiece(string playerGuid);
        IMessage TestPiece(string playerGuid);
    }
}