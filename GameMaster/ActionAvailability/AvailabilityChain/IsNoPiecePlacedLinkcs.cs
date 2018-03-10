using GameMaster.ActionAvailability.ActionAvailabilityHelpers;
using Shared.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster.ActionAvailability.AvailabilityChain
{
    class IsNoPiecePlacedLinkcs : AvailabilityChainBase {
        Location location;
        Board board;
        public IsNoPiecePlacedLinkcs(Location location, Board board) {
            this.location = location;
            this.board = board;
        }

        protected override bool Validate() {
            return !PieceRelatedAvailability.IsPieceInCurrentLocation(location, board);
        }
    }
}
