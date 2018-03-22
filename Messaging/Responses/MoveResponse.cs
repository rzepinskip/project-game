using System.Collections.Generic;
using Common.BoardObjects;

namespace Messaging.Responses
{
    internal class MoveResponse : Response
    {
        public int PlayerId { get; set; }
        public List<TaskField> TaskFields { get; set; }
        public List<Piece> Pieces { get; set; }
        public Location NewPlayerLocation { get; internal set; }
    }
}