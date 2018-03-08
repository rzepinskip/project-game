using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Shared.BoardObjects
{
    [Serializable]
    public class GoalField : Field
    {
        [XmlAttribute("type")]
        public Shared.CommonResources.GoalFieldType Type { get; set; }

        [XmlAttribute("team")]
        public Shared.CommonResources.Team Team { get; set; }
    }
}
