using GameMaster.ActionAvailability.AvailabilityLink;
using System.Collections.Generic;

namespace GameMaster.ActionAvailability.AvailabilityChain
{
    public class TestAvailabilityChain : IAvailabilityChain
    {
        private string playerGuid;
        private Dictionary<string, int> playerGuidToPiece;

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
