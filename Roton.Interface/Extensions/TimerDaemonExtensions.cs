using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
