using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace Player.Logging
{
    interface ILoggerFactory
    {
        ILogger GetPlayerLogger(int id);
    }
}
