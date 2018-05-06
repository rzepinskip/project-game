using NLog;

namespace Player.Logging
{
    internal interface ILoggerFactory
    {
        ILogger GetPlayerLogger(int id);
    }
}