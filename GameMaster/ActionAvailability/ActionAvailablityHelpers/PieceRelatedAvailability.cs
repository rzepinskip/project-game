using Shared.Board;
using System.Collections.Generic;

namespace GameMaster.ActionAvailability.ActionAvailabilityHelpers
{
    public static class PieceRelatedAvailability
    {
        public static bool IsPieceInCurrentLocation(Location l, Board board)
        {
            var PieceId = board.GetPieceFromBoard(l);
            var result = true;
            if (PieceId == null)
                result = false;
            return result;
        }
        public static bool HasPlayerEmptySlotForPiece(string playerGuid, Dictionary<string, int> playerGuidToPiece)
        {
            var result = true;
            if (playerGuidToPiece.TryGetValue(playerGuid, out var pieceId))
                result = false;
            return result;
        }
    }
}
