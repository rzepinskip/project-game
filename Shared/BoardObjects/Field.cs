using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Shared.BoardObjects
{
    [Serializable]
    public abstract class Field : Location
    {
        [XmlAttribute()]
        public DateTime Timestamp { get; set; }

        [XmlAttribute()]
        public string PlayerId { get; set; }

        [XmlAttribute()]
        public bool PlayerIdFieldSpecified { get; set; }
    }
}
