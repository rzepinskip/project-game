using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{

    public interface ILoggable
    {
        ActionLog ToLog(int playerId, PlayerInfo playeInfo);
    }
}
