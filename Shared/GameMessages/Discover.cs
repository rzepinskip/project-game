using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Shared.BoardObjects;
using System.Xml.Serialization;
using Shared.ResponseMessages;

namespace Shared.GameMessages
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class Discover : GameMessage
    {
        public override ResponseMessage Execute(Board board)
        {
            throw new NotImplementedException();
        }
    }
}
