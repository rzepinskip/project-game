﻿using GameMaster.ActionAvailability.ActionAvailabilityHelpers;
using Shared.Board;

namespace GameMaster.ActionAvailability.AvailabilityLink
{
    class IsNoPiecePlacedLink : AvailabilityLinkBase
    {
        private Location location;
        private Board board;
        public IsNoPiecePlacedLink(Location location, Board board)
        {
            this.location = location;
            this.board = board;
        }

        protected override bool Validate()
        {
            return !PieceRelatedAvailability.IsPieceInCurrentLocation(location, board);
        }
    }
}
