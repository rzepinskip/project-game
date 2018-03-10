using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster.ActionAvailability.AvailabilityChain
{
    class HasPlayerEmptySlotForPieceLink : AvailabilityChainBase
    {
        public string playerGuid;
        public Dictionary<string, int> playerGuidToPieceId;
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
