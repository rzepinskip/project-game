using Shared.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster.ActionAvailability
{
    public static class PlaceAvailability
    {
        public static bool IsNoPiecePlaced(Location l, Board board) {
            int? PieceId = board.GetPieceFromBoard(l);
            bool result = false;
            if (PieceId == null)
                result = true;
            return result;
        }

        public static bool IsPlayerCarryingPiece(string playerGuid, Dictionary<string, int> playerGuidToPiece) {
            bool result = false;
            int pieceId;
            if (playerGuidToPiece.TryGetValue(playerGuid, out pieceId))
                result = true;
            return result;
        }
    }
}
