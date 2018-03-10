using Shared.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster.ActionAvailability.ActionAvailabilityHelpers
{
    public static class PieceRelatedAvailability
    {
        public static bool IsPieceInCurrentLocation(Location l, Board board)
        {
            int? PieceId = board.GetPieceFromBoard(l);
            if (PieceId == null)
                return false;
            return true;
        }
        public static bool HasPlayerEmptySlotForPiece(string playerGuid, Dictionary<string, int> playerGuidToPiece)
        {
            bool result = true;
            int pieceId;
            if (playerGuidToPiece.TryGetValue(playerGuid, out pieceId))
                result = false;
            return result;
        }
    }
}
