using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Shared.Action
{
    public class MoveAction : IAction
    {
        public int X { get; set; }
        public int Y { get; set; }

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
