using GameMaster.ActionAvailability.ActionAvailabilityHelpers;
using Shared.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster.ActionAvailability.AvailabilityChain
{
    class IsPieceInCurrentLocationLink : AvailabilityChainBase
    {
        Location location;
        Board board;
        public IsPieceInCurrentLocationLink(Location location, Board board)
        {
            this.location = location;
            this.board = board;
        }

        protected override bool Validate()
        {
            return PieceRelatedAvailability.IsPieceInCurrentLocation(location, board);
        }
    }
}
