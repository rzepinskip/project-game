using Shared.BoardObjects;
using System.Collections.Generic;

namespace Shared.ActionAvailability.ActionAvailabilityHelpers
{
    public class PieceRelatedAvailability
    {
        public bool IsPieceInCurrentLocation(Location l, Board board)
        {
            var PieceId = board.GetPieceFromBoard(l);
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
