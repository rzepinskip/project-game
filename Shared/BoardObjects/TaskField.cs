using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Shared.BoardObjects
{
    [Serializable]
    public class TaskField: Field
    {
        [XmlAttribute()]
        public int DistanceToPiece { get; set; }

        [XmlAttribute()]
        public ulong PieceId { get; set; }

        [XmlIgnore()]
        public bool PieceIdSpecified { get; set; }
    }
}
