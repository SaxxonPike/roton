using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;

namespace Roton.Extensions
{
    public static class TimerExtensions
    {
        public static bool Clock(this ITimer timer, int interval)
        {
            var value = timer.Ticks + 1;
            var result = false;

            if (value >= interval)
            {
                value = 0;
                result = true;
            }

            timer.Ticks = value;
            return result;
        }
    }
}
