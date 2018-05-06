using System;
using System.Collections.Generic;
using System.Text;
using ClientsCommon.ActionAvailability.AvailabilityLink;
using Common;

namespace ClientsCommon.ActionAvailability.AvailabilityChain
{
    public class DestroyAvailabilityChain : IAvailabilityChain
    {
        private readonly int _playerId;
        private readonly Dictionary<int, PlayerInfo> _players;

        public DestroyAvailabilityChain(int playerId, Dictionary<int, PlayerInfo> players)
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
