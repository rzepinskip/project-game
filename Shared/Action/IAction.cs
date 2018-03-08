using Shared.BoardObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Shared.Action
{
    public interface IAction
    {
        void Execute(Board board);
        void CanExecute(Board board);
    }
}
