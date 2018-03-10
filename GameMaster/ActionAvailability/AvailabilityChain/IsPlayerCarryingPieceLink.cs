using GameMaster.ActionAvailability.ActionAvailabilityHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster.ActionAvailability.AvailabilityChain
{
    class IsPlayerCarryingPieceLink : AvailabilityChainBase
    {
        public string playerGuid;
        public Dictionary<string, int> playerGuidToPieceId;
        public IsPlayerCarryingPieceLink(string playerGuid, Dictionary<string, int> playerGuidToPieceId) {
            this.playerGuid = playerGuid;
            this.playerGuidToPieceId = playerGuidToPieceId;
        }
        protected override bool Validate() {
            return !PieceRelatedAvailability.HasPlayerEmptySlotForPiece(playerGuid, playerGuidToPieceId);
        }
    }
}
