using System.Collections.Generic;
using Common.ActionAvailability.AvailabilityLink;

namespace Common.ActionAvailability.AvailabilityChain
{
    public class TestAvailabilityChain : IAvailabilityChain
    {
        private readonly int _playerId;
        private readonly Dictionary<int, PlayerInfo> _players;

        public TestAvailabilityChain(int playerId, Dictionary<int, PlayerInfo> players)
        {
            _playerId = playerId;
            _players = players;
        }

        public bool ActionAvailable()
        {
            var builder = new AvailabilityChainBuilder(new IsPlayerCarryingPieceLink(_playerId, _players));
            return builder.Build().ValidateLink();
        }
    }
}