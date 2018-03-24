using System.Collections.Generic;
using Common.BoardObjects;
using Common.Interfaces;

namespace Messaging.Responses
{
    public class MoveResponse : Response
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

            foreach (var taskField in TaskFields)
                board[taskField] = taskField;

            foreach (var piece in Pieces)
                board.Pieces[piece.Id] = piece;

            board[NewPlayerLocation].PlayerId = PlayerId;
        }
    }
}