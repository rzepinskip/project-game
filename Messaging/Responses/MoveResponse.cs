using System.Collections.Generic;
using Common.BoardObjects;
using Common.Interfaces;

namespace Messaging.Responses
{
    internal class MoveResponse : Response
    {
        public MoveResponse(int playerId, Location newPlayerLocation, IEnumerable<TaskField> taskFields,
            IEnumerable<Piece> pieces, bool isGameFinished = false) : base(playerId, isGameFinished)
        {
            NewPlayerLocation = newPlayerLocation;
            TaskFields = taskFields;
            Pieces = pieces;
        }

        public Location NewPlayerLocation { get; }
        public IEnumerable<TaskField> TaskFields { get; }
        public IEnumerable<Piece> Pieces { get; }

        public override void Update(IBoard board)
        {
            var playerInfo = board.Players[PlayerId];
            var currentLocation = playerInfo.Location;
            board[currentLocation].PlayerId = null;

            playerInfo.Location = NewPlayerLocation;
            board[NewPlayerLocation].PlayerId = PlayerId;
        }
    }
}