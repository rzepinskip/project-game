using Shared.BoardObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Shared.GameMessages
{

    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class Move : GameMessage
    {
        [XmlAttribute()]
        public CommonResources.MoveType Direction { get; set; }

        [XmlAttribute()]
        public bool DirectionFieldSpecified;

        public override void Execute(BoardObjects.Board board)
        {
            throw new NotImplementedException();
        }

        public override void CanExecute(BoardObjects.Board board)
        {
            throw new NotImplementedException();
        }
    }
}
