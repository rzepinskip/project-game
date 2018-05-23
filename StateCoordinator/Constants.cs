using System;

namespace PlayerStateCoordinator
{
    public static class Constants
    {
        public static readonly TimeSpan DefaultStateTimeout = TimeSpan.FromSeconds(10);
        public const int DefaultLastStatesChecked = 30;

    }
}