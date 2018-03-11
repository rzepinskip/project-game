using GameMaster.ActionAvailability.ActionAvailabilityHelpers;
using System.Collections.Generic;

namespace GameMaster.ActionAvailability.AvailabilityLink
{
    class IsPlayerCarryingPieceLink : AvailabilityLinkBase
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
