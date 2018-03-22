using System.Collections.Generic;
using Shared.ActionAvailability.AvailabilityLink;
using Shared.BoardObjects;

namespace Shared.ActionAvailability.AvailabilityChain
{
    public class PlaceAvailabilityChain : IAvailabilityChain
    {
        private readonly Board board;
        private readonly Location location;
        private readonly string playerGuid;
        private readonly Dictionary<string, int> playerGuidToPiece;

        public PlaceAvailabilityChain(Location location, Board board, string playerGuid,
            Dictionary<string, int> playerGuidToPiece)
        {
            this.location = location;
            this.board = board;
            this.playerGuid = playerGuid;
            this.playerGuidToPiece = playerGuidToPiece;
        }

        public bool ActionAvailable()
        {
            var builder = new AvailabilityChainBuilder(new IsNoPiecePlacedLink(location, board))
                .AddNextLink(new IsPlayerCarryingPieceLink(playerGuid, playerGuidToPiece));
            return builder.Build().ValidateLink();
        }
    }
}