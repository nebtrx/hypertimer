using System.Runtime.InteropServices;

namespace System.Timers.Utilities
{
    internal static class WinAPIHelper
    {
        [DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceCounter(out long count);

        [DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceFrequency(out long frequency);

        [DllImport("Kernel32.dll")]
        public static extern void GetSystemTimePreciseAsFileTime(out long time);
    }
}
