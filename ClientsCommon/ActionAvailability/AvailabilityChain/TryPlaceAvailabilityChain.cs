using ClientsCommon.ActionAvailability.AvailabilityLink;
using Common.BoardObjects;
using Common.Interfaces;

namespace ClientsCommon.ActionAvailability.AvailabilityChain
{
    public class TryPlaceAvailabilityChain : IAvailabilityChain
    {
        private readonly IBoard _board;
        private readonly Location _location;
        private readonly int _playerId;

        public TryPlaceAvailabilityChain(Location location, IBoard board, int playerId)
        {
            _location = location;
            _board = board;
            _playerId = playerId;
        }

        public bool ActionAvailable()
        {
            var builder = new AvailabilityChainBuilder(new IsPlayerCarryingPieceLink(_playerId, _board.Players));
            return builder.Build().ValidateLink();
        }
    }
}