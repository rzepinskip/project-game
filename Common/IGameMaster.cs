using System.Runtime.InteropServices.ComTypes;
using Shared;

namespace Common
{
    public interface IGameMaster
    {
        IMessage Discover(string playerGuid);
        IMessage Move(string playerGuid, CommonResources.MoveType direction);
        IMessage PickUpPiece(string playerGuid);
        IMessage PlacePiece(string playerGuid);
        IMessage TestPiece(string playerGuid);
    }
}