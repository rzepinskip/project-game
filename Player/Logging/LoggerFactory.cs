using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets;
using Player.Logging;

namespace Player
{
    public class LoggerFactory
    {
        public ILogger GetPlayerLogger(int id)
        {
            var loggerName = $"player{id}";
            var config = LogManager.Configuration;

            if (config.LoggingRules.FirstOrDefault(r => r.NameMatches(loggerName)) == null)
            {
                var logfile = new FileTarget
                {
                    FileName = $"${{basedir}}/logs/players/player{id}.csv",
                    Layout = "${longdate},${message}",

                };
                config.LoggingRules.Add(new LoggingRule(loggerName, LogLevel.Info, logfile));
                LogManager.Configuration = config;
            }

            return LogManager.GetLogger(loggerName);
        }
    }
}
