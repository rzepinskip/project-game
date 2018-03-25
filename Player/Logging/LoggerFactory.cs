using System.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Player.Logging
{
    public class LoggerFactory
    {
        public ILogger GetPlayerLogger(int id)
        {
            var loggerName = $"Player.Player_{id}";
            var config = LogManager.Configuration;

            if (config.LoggingRules.FirstOrDefault(r => r.NameMatches(loggerName)) == null)
            {
                var logfile = new FileTarget
                {
                    FileName = $"${{basedir}}/logs/players/player{id}_${{longdate:cached=true}}.csv",
                    ArchiveFileName = $"${{basedir}}/logs/archives/players/player{id}_${{longdate:cached=true}}.csv",
                    ArchiveEvery = FileArchivePeriod.Day,
                    Layout = "${longdate},${message}",
                };
                config.LoggingRules.Add(new LoggingRule(loggerName, LogLevel.Info, logfile));
                LogManager.Configuration = config;
            }

            return LogManager.GetLogger(loggerName);
        }
    }
}
