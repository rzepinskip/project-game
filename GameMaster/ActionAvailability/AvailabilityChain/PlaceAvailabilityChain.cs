using GameMaster.ActionAvailability.AvailabilityLink;
using Shared.Board;
using System.Collections.Generic;

namespace GameMaster.ActionAvailability.AvailabilityChain
{
    public class PlaceAvailabilityChain : IAvailabilityChain
    {
        private Location location;
        private Board board;
        private string playerGuid;
        private Dictionary<string, int> playerGuidToPiece;

        public PlaceAvailabilityChain(Location location, Board board, string playerGuid, Dictionary<string, int> playerGuidToPiece)
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
