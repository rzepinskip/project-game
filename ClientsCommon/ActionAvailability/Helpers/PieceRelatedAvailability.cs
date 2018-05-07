using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace ClientsCommon.ActionAvailability.Helpers
{
    public class PieceRelatedAvailability
    {
        public bool IsPieceInCurrentLocation(Location l, IBoard board)
        {
            var PieceId = board.GetPieceIdAt(l);
            return PieceId != null;
        }

        public bool HasPlayerEmptySlotForPiece(int playerId, Dictionary<int, PlayerInfo> players)
        {
            var result = true;
            var playerInfo = players[playerId];
            if (playerInfo?.Piece != null)
                result = false;
            return result;
        }
    }
}