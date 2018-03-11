using GameMaster.ActionAvailability.AvailabilityChain;
using System.Collections.Generic;
using Xunit;

namespace GameMaster.Tests
{
    public class TestAvailabilityChainTests
    {

        string playerGuidSuccessTest = "c094cab7-da7b-457f-89e5-a5c51756035f";
        string playerGuidFailTest = "c094cab7-da7b-457f-89e5-a5c51756035d";
        Dictionary<string, int> playerGuidToPieceId;
        int pieceId = 1;

        Shared.Board.Board board;

        public TestAvailabilityChainTests()
        {

            playerGuidToPieceId = new Dictionary<string, int>();
            playerGuidToPieceId.Add(playerGuidSuccessTest, pieceId);
        }

        [Fact]
        public void PlayerTestWhenCarryingPiece()
        {
            var testAvailabilityChain = new TestAvailabilityChain(playerGuidSuccessTest, playerGuidToPieceId);
            Assert.True(testAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void PlayerTestWhenCarryingNoPiece()
        {
            var testAvailabilityChain = new TestAvailabilityChain(playerGuidFailTest, playerGuidToPieceId);
            Assert.False(testAvailabilityChain.ActionAvailable());
        }


    }
}
