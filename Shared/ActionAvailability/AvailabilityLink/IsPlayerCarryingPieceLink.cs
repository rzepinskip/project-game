using Shared.ActionAvailability.ActionAvailabilityHelpers;
using System.Collections.Generic;

namespace Shared.ActionAvailability.AvailabilityLink
{
    class IsPlayerCarryingPieceLink : AvailabilityLinkBase
    {
        private string playerGuid;
        private Dictionary<string, int> playerGuidToPieceId;
        public IsPlayerCarryingPieceLink(string playerGuid, Dictionary<string, int> playerGuidToPieceId)
        {
            this.playerGuid = playerGuid;
            this.playerGuidToPieceId = playerGuidToPieceId;
        }
        protected override bool Validate()
        {
            return !PieceRelatedAvailability.HasPlayerEmptySlotForPiece(playerGuid, playerGuidToPieceId);
        }
    }
}
