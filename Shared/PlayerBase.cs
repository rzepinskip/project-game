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

        public CommonResources.TeamColour Team { get; set; }
        public PlayerType Type { get; set; }
        public int Id { get; set; }
    }
}
