using Shared.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster.ActionAvailability
{
    public static class PickUpAvailability
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
            return true;
        }
    }
}
