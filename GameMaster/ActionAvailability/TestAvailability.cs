using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster.ActionAvailability
{
    public static class TestAvailability
    {
        public static bool IsPlayerCarryingPiece(string playerGuid, Dictionary<string, int> playerGuidToPiece) {
            bool result = false;
            int pieceId;
            if (playerGuidToPiece.TryGetValue(playerGuid, out pieceId))
                result = true;
            return result;
        }
    }
}
