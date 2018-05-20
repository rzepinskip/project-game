using System;

namespace Messaging
{
    public static class Constants
    {
        public const string DefaultMessagingNameSpace = "https://se2.mini.pw.edu.pl/17-results/";
        public static readonly TimeSpan DefaultRequestRetryInterval = TimeSpan.FromSeconds(1);
    }
}