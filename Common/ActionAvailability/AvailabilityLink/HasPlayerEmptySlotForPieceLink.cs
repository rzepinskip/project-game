using System.Collections.Generic;
using Common.ActionAvailability.ActionAvailabilityHelpers;
using Common.ActionAvailability.AvailabilityLink;

namespace Commons.ActionAvailability.AvailabilityLink
{
    internal class HasPlayerEmptySlotForPieceLink : AvailabilityLinkBase
    {
        private readonly string playerGuid;
        private readonly Dictionary<string, int> playerGuidToPieceId;

        public HasPlayerEmptySlotForPieceLink(string playerGuid, Dictionary<string, int> playerGuidToPieceId)
        {
            this.playerGuid = playerGuid;
            this.playerGuidToPieceId = playerGuidToPieceId;
        }

        protected override bool Validate()
        {
            return new PieceRelatedAvailability().HasPlayerEmptySlotForPiece(playerGuid, playerGuidToPieceId);
        }
    }
}