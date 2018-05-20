using Common.BoardObjects;

namespace Common.Interfaces
{
    public interface IBoard
    {
        SerializableDictionary<int, PlayerInfo> Players { get; }

        int TaskAreaSize { get; }
        int GoalAreaSize { get; }
        int Width { get; }
        int Height { get; }

        Field this[Location location] { get; set; }

        int? GetPieceIdAt(Location location);
        bool IsLocationInTaskArea(Location location);
        int DistanceToPieceFrom(Location location);
    }
}