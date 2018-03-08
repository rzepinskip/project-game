using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Board
{
    public abstract class Field : Location {
        //public Location Location { get; set; }
        public int? PlayerId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
