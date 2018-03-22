using System.Collections.Generic;
using Common.ActionAvailability.Helpers;

namespace Common.ActionAvailability.AvailabilityLink
{
    internal class IsPlayerCarryingPieceLink : AvailabilityLinkBase
    {
        private readonly string playerGuid;
        private readonly Dictionary<string, int> playerGuidToPieceId;

        public IsPlayerCarryingPieceLink(string playerGuid, Dictionary<string, int> playerGuidToPieceId)
        {
            this.playerGuid = playerGuid;
            this.playerGuidToPieceId = playerGuidToPieceId;
        }

        protected override bool Validate()
        {
            return !new PieceRelatedAvailability().HasPlayerEmptySlotForPiece(playerGuid, playerGuidToPieceId);
        }
    }
}