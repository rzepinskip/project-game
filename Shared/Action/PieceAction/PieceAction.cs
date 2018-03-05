using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Shared.Action.PieceAction
{
    public abstract class PieceAction : IAction
    {
        public MockPiece Piece { get; set; }

        public void CanExecute(MockBoard board)
        {
            throw new NotImplementedException();
        }

        public void Execute(MockBoard board)
        {
            throw new NotImplementedException();
        }
    }
}
