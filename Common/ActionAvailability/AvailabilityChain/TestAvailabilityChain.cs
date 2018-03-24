using System.Collections.Generic;
using Common.ActionAvailability.AvailabilityLink;

namespace Common.ActionAvailability.AvailabilityChain
{
    public class TestAvailabilityChain : IAvailabilityChain
    {
        private readonly string playerGuid;
        private readonly Dictionary<string, int> playerGuidToPiece;

        public TestAvailabilityChain(string playerGuid, Dictionary<string, int> playerGuidToPiece)
        {
            this.playerGuid = playerGuid;
            this.playerGuidToPiece = playerGuidToPiece;
        }

        public bool ActionAvailable()
        {
            var builder = new AvailabilityChainBuilder(new IsPlayerCarryingPieceLink(playerGuid, playerGuidToPiece));
            return builder.Build().ValidateLink();
        }
    }
}