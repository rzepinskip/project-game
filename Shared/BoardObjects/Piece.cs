using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Shared.BoardObjects
{
    [Serializable]
    public class Piece
    {
        [XmlAttribute()]
        public int Id { get; set; }

        [XmlAttribute()]
        public CommonResources.PieceType Type { get; set; }

        [XmlAttribute()]
        public DateTime Timestamp { get; set; }

        [XmlAttribute()]
        public string PlayerId { get; set; }

        [XmlAttribute()]
        public bool PlayerIdFieldSpecified { get; set; }
    }
}
