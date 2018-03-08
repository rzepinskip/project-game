using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.BoardObjects
{
    public class TaskField: Field
    {
        public int DistanceToPiece { get; set; }
        public int? PieceId { get; set; }
    }
}
