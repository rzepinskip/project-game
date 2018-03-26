using Common.BoardObjects;

namespace Common.Interfaces
{
    public interface IGameMaster
    {
        IGameMasterBoard Board { get; set; }

        bool IsDiscoverPossible(string playerGuid);
        bool IsMovePossible(string playerGuid, Direction direction);
        bool IsPickUpPiecePossible(string playerGuid);
        bool IsPlacePiecePossible(string playerGuid);
        bool IsTestPiecePossible(string playerGuid);

        Location GetPlayerLocation(string playerGuid);
    }
}