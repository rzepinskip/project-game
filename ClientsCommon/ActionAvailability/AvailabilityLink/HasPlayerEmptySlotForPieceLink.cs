using System.Collections.Generic;
using ClientsCommon.ActionAvailability.Helpers;
using Common;

namespace ClientsCommon.ActionAvailability.AvailabilityLink
{
    internal class HasPlayerEmptySlotForPieceLink : AvailabilityLinkBase
    {
        private readonly int _playerId;
        private readonly Dictionary<int, PlayerInfo> _players;

        public HasPlayerEmptySlotForPieceLink(int playerId, Dictionary<int, PlayerInfo> players)
        {
            _playerId = playerId;
            _players = players;
        }

        protected override bool Validate()
        {
            return new PieceRelatedAvailability().HasPlayerEmptySlotForPiece(_playerId, _players);
        }
    }
}