using GameMaster.ActionAvailability.ActionAvailabilityHelpers;
using Shared.Board;
using System;
using System.Collections.Generic;
using System.Text;
using static Shared.CommonResources;

namespace GameMaster.ActionAvailability.AvailabilityLink
{
    class IsFieldPlayerUnoccupiedLink : AvailabilityLinkBase
    {
        Location location;
        MoveType move;
        Board board;
        public IsFieldPlayerUnoccupiedLink(Location location, MoveType move, Board board)
        {
            this.location = location;
            this.move = move;
            this.board = board;
        }
        protected override bool Validate()
        {
            return MoveAvailability.IsFieldPlayerUnoccupied(location, move, board);
        }
    }
}
