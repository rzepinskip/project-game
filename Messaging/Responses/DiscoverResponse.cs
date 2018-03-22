using System.Collections.Generic;
using Common.BoardObjects;
using Common.Interfaces;

namespace Messaging.Responses
{
    internal class DiscoverResponse : Response
    {
        public DiscoverResponse(int playerId, IEnumerable<TaskField> taskFields, IEnumerable<Piece> pieces,
            bool isGameFinished = false) : base(playerId)
        {
            TaskFields = taskFields;
            Pieces = pieces;
        }

        public IEnumerable<TaskField> TaskFields { get; }
        public IEnumerable<Piece> Pieces { get; }

        public override void Update(IBoard board)
        {
            foreach (var taskField in TaskFields)
                board[taskField] = taskField;
        }
    }
}