using System.Collections;
using System.Collections.Generic;
using Common.BoardObjects;

namespace Common.Interfaces
{
    public interface IBoard : IEnumerable
    {
        Dictionary<int, PlayerInfo> Players { get; }
        Dictionary<int, Piece> Pieces { get; }

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