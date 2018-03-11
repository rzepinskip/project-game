using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Shared.BoardObjects
{
    public class TaskField : Field
    {
        public int DistanceToPiece { get; set; }
        public int? PieceId { get; set; }
    }
}
