using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Shared.BoardObjects;

namespace Shared.Action
{
    public class DiscoverAction : IAction
    {
        public int X { get; set; }
        public int Y { get; set; }

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
