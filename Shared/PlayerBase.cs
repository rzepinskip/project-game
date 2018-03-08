using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Shared
{
    public class PlayerBase
    {
        public enum PlayerType {
            [XmlEnum(Name = "member")]
            Member,
            [XmlEnum(Name = "leader")]
            Leader
        }

        private CommonResources.TeamColour Team { get; set; }
        private PlayerType Type { get; set; }
        private int Id { get; set; }
    }
}
