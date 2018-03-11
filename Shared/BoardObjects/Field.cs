using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Shared.BoardObjects
{
    public abstract class Field : Location
    {
        //public Location Location { get; set; }
        public int? PlayerId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
