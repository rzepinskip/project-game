using System.Collections.Generic;
using Common.ActionAvailability.AvailabilityLink;
using Common.BoardObjects;
using Common.Interfaces;

namespace Common.ActionAvailability.AvailabilityChain
{
    public class PlaceAvailabilityChain : IAvailabilityChain
    {
        private readonly IBoard board;
        private readonly Location location;
        private readonly string playerGuid;
        private readonly Dictionary<string, int> playerGuidToPiece;

        public PlaceAvailabilityChain(Location location, IBoard board, string playerGuid,
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