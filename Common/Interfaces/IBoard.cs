using Common.BoardObjects;

namespace Common.Interfaces
{
    public interface IBoard
    {
        SerializableDictionary<int, PlayerInfo> Players { get; }
        SerializableDictionary<int, Piece> Pieces { get; }

        int TaskAreaSize { get; }
        int GoalAreaSize { get; }
        int Width { get; }
        int Height { get; }

        Field this[Location location] { get; set; }

        int? GetPieceIdAt(Location llocation);
        bool IsLocationInTaskArea(Location location);
        int DistanceToPieceFrom(Location location);
    }
}