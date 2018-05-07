using ClientsCommon.ActionAvailability.AvailabilityLink;
using Common.BoardObjects;
using Common.Interfaces;

namespace ClientsCommon.ActionAvailability.AvailabilityChain
{
    public class PlaceAvailabilityChain : IAvailabilityChain
    {
        private readonly IBoard _board;
        private readonly Location _location;
        private readonly int _playerId;

        public PlaceAvailabilityChain(Location location, IBoard board, int playerId)
        {
            _location = location;
            _board = board;
            _playerId = playerId;
        }

        public bool ActionAvailable()
        {
            var builder = new AvailabilityChainBuilder(new IsNoPiecePlacedLink(_location, _board))
                .AddNextLink(new IsPlayerCarryingPieceLink(_playerId, _board.Players));
            return builder.Build().ValidateLink();
        }
    }
}