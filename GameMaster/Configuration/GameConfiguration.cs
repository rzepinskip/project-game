using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Shared.Board;

namespace GameMaster.Configuration
{
    [XmlRoot(ElementName = "GameMasterSettings", Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    public class GameConfiguration
    {
        public GameDefinition GameDefinition { get; set; }
        public ActionCosts ActionCosts { get; set; }

        [XmlAttribute]
        public double KeepAliveInterval { get; set; }

        [XmlAttribute]
        public double RetryRegisterGameInterval { get; set; }
    }

    

}
