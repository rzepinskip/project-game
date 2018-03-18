using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.GameMessages
{
    public interface IDelayed
    {
        double GetDelay(ActionCosts actionCosts);
    }
}
