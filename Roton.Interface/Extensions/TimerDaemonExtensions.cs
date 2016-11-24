using System;
using Roton.Interface.Synchronization;

namespace Roton.Interface.Extensions
{
    public static class TimerDaemonExtensions
    {
        public static ITimedEvent CreateTimedEvent(this ITimerDaemon daemon, Action action, double frequency)
        {
            return new TimedEvent(daemon, action, frequency);
        }
    }
}
