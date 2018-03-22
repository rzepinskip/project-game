using System.Collections.Generic;
using Shared.BoardObjects;

namespace Shared.ResponseMessages
{
    public class DiscoverResponse : ResponseMessage
    {
        public IEnumerable<TaskField> TaskFields { get; set; }
        public IEnumerable<Piece> Pieces { get; set; }

        public override void Update(Board board)
        {
            foreach (var taskField in TaskFields)
                board.Content[taskField.X, taskField.Y] = taskField;
        }
    }
}