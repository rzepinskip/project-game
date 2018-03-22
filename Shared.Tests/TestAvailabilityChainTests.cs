using System.Collections.Generic;
using Shared.ActionAvailability.AvailabilityChain;
using Xunit;

namespace Shared.Tests
{
    public class TestAvailabilityChainTests
    {
        public TestAvailabilityChainTests()
        {
            playerGuidToPieceId = new Dictionary<string, int>();
            playerGuidToPieceId.Add(playerGuidSuccessTest, pieceId);
        }

        private readonly string playerGuidSuccessTest = "c094cab7-da7b-457f-89e5-a5c51756035f";
        private readonly string playerGuidFailTest = "c094cab7-da7b-457f-89e5-a5c51756035d";
        private readonly Dictionary<string, int> playerGuidToPieceId;
        private readonly int pieceId = 1;

        [Fact]
        public void PlayerTestWhenCarryingNoPiece()
        {
            var testAvailabilityChain = new TestAvailabilityChain(playerGuidFailTest, playerGuidToPieceId);
            Assert.False(testAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void PlayerTestWhenCarryingPiece()
        {
            var testAvailabilityChain = new TestAvailabilityChain(playerGuidSuccessTest, playerGuidToPieceId);
            Assert.True(testAvailabilityChain.ActionAvailable());
        }
    }
}