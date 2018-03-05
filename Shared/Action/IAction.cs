using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Shared.Action
{
    public interface IAction
    {
        void Execute(MockBoard board);
        void CanExecute(MockBoard board);
    }
}
