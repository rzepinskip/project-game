using System.Collections.Generic;
using Common.BoardObjects;
using Common.Interfaces;

namespace Common.ActionAvailability.Helpers
{
    public class PieceRelatedAvailability
    {
        public bool IsPieceInCurrentLocation(Location l, IBoard board)
        {
            var PieceId = board.GetPieceIdAt(l);
            var result = true;
            if (PieceId == null)
                result = false;
            return result;
        }

        public bool HasPlayerEmptySlotForPiece(string playerGuid, Dictionary<string, int> playerGuidToPiece)
        {
            var result = true;
            if (playerGuidToPiece.TryGetValue(playerGuid, out var pieceId))
                result = false;
            return result;
        }
    }
}