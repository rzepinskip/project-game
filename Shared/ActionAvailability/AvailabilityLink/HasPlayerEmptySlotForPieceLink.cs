using Shared.ActionAvailability.ActionAvailabilityHelpers;
using System.Collections.Generic;

namespace Shared.ActionAvailability.AvailabilityLink
{
    class HasPlayerEmptySlotForPieceLink : AvailabilityLinkBase
    {
        private string playerGuid;
        private Dictionary<string, int> playerGuidToPieceId;
        public HasPlayerEmptySlotForPieceLink(string playerGuid, Dictionary<string, int> playerGuidToPieceId)
        {
            this.playerGuid = playerGuid;
            this.playerGuidToPieceId = playerGuidToPieceId;
        }
        protected override bool Validate()
        {
            return PieceRelatedAvailability.HasPlayerEmptySlotForPiece(playerGuid, playerGuidToPieceId);
        }
    }
}
