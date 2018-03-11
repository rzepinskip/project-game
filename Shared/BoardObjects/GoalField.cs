using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Shared.BoardObjects
{
    [Serializable]
    public class GoalField : Field
    {
        public Shared.CommonResources.GoalFieldType Type { get; set; }
        public Shared.CommonResources.TeamColour Team { get; set; }
    }
}

