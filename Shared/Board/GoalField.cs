using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Board
{
    class GoalField : Field
    {
        public Shared.CommonResources.GoalFieldType Type { get; set; }
        public Shared.CommonResources.Team Team { get; set; }
    }
}
