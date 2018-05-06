using System;

namespace Communication
{
    public static class Constants
    {
        public static readonly TimeSpan DefaultMaxUnresponsivenessDuration = TimeSpan.FromSeconds(30);
        public const int BufferSize = 1024;
        public const char EtbByte = (char) 23;
        public const int KeepAliveIntervalFrequencyDivisor = 8;
    }
}
