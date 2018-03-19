using System.Collections.Generic;
using Shared.ActionAvailability.AvailabilityLink;
using Shared.BoardObjects;

namespace Shared.ActionAvailability.AvailabilityChain
{
    public class PickUpAvailabilityChain
    {
        private readonly Board board;
        private readonly Location location;
        private readonly string playerGuid;
        private readonly Dictionary<string, int> playerGuidToPiece;

        public PickUpAvailabilityChain(Location location, Board board, string playerGuid,
            Dictionary<string, int> playerGuidToPiece)
        {
            this.location = location;
            this.board = board;
            this.playerGuid = playerGuid;
            this.playerGuidToPiece = playerGuidToPiece;
        }

        public bool ActionAvailable()
        {
            var builder = new AvailabilityChainBuilder(new IsPieceInCurrentLocationLink(location, board))
                .AddNextLink(new HasPlayerEmptySlotForPieceLink(playerGuid, playerGuidToPiece));
            return builder.Build().ValidateLink();
        }
    }
}