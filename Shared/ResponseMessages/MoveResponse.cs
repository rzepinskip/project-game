using System;
using System.Collections.Generic;
using System.Text;
using Shared.BoardObjects;

namespace Shared.ResponseMessages
{
    public class MoveResponse : ResponseMessage
    {
        public IEnumerable<TaskField> TaskFields { get; set; }
        public IEnumerable<Piece> Pieces { get; set; }
        public Location NewPlayerLocation { get; set; }

        public override void Update(Board board)
        {
            var playerInfo = board.Players[this.PlayerId];
            var currentLocation = playerInfo.Location;
            board.Content[currentLocation.X, currentLocation.Y].PlayerId = null;

            playerInfo.Location = NewPlayerLocation;
            board.Content[NewPlayerLocation.X, NewPlayerLocation.Y].PlayerId = this.PlayerId;
        }
    }
}
