using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Board
{
    abstract class Field : BoardObject {
        public FieldObject ContainedObject { get; set; }
        public Pawn PlayerPawn { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
