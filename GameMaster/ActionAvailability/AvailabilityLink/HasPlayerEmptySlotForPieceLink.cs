﻿using GameMaster.ActionAvailability.ActionAvailabilityHelpers;
using System.Collections.Generic;

namespace GameMaster.ActionAvailability.AvailabilityLink
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
