using Shared.BoardObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Shared.Action.PieceActions
{
    public abstract class PieceAction : IAction
    {
        public Piece Piece { get; set; }

        public void CanExecute(Board board)
        {
            throw new NotImplementedException();
        }

        public void Execute(Board board)
        {
            throw new NotImplementedException();
        }
    }
}
