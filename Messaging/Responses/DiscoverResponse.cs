using System.Collections.Generic;
using Common.BoardObjects;

namespace Messaging.Responses
{
    internal class DiscoverResponse : Response
    {
        public int PlayerId { get; set; }
        public List<TaskField> TaskFields { get; set; }
        public List<Piece> Pieces { get; set; }
    }
}