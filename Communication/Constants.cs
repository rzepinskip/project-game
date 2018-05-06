using System;

namespace Communication
{
    public static class Constants
    {
        public static readonly TimeSpan DefaultKeepAliveInterval = TimeSpan.FromSeconds(30);
        public const int BufferSize = 1024;
        public const char EtbByte = (char) 23;
    }
}
