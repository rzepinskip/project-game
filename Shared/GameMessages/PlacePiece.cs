using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Shared.BoardObjects;

namespace Shared.GameMessages.PieceActions
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class PlacePiece : GameMessage
    {
        public override void CanExecute(BoardObjects.Board board)
        {
            throw new NotImplementedException();
        }

        public override void Execute(BoardObjects.Board board)
        {
            throw new NotImplementedException();
        }
    }

}
