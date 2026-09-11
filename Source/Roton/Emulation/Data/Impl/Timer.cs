using System;
using Roton.Emulation.Core;

namespace Roton.Emulation.Data.Impl;

internal sealed class Timer(
    IMemory memory,
    int offset,
    IConfig config)
    : ITimer
{
    private const long TsTicksPerHsec = 10 * TimeSpan.TicksPerMillisecond;

    public TimeSpan LastUpdate
    {
        get;
        set
        {
            field = value;
            Value = unchecked((short)(value.Ticks / TsTicksPerHsec));
        }
    }

    public ref Word Value =>
        ref memory.GetRef<Word>(offset);

    private bool Update(TimeSpan totalElapsed, TimeSpan interval)
    {
        var nextValue = LastUpdate + interval;
        if (nextValue > totalElapsed)
            return false;

        LastUpdate = totalElapsed;
        return true;
    }

    public bool UpdateByGameTicks(TimeSpan totalElapsed, int intervalGameTicks)
    {
        var intervalTsTicks = TsTicksPerHsec * intervalGameTicks *
                              config.Engine.MasterClockNumerator * 100 /
                              config.Engine.MasterClockDenominator;
        var intervalTs = TimeSpan.FromTicks(intervalTsTicks);
        return Update(totalElapsed, intervalTs);
    }

    public bool UpdateByRealTime(TimeSpan totalElapsed, int intervalHsec)
    {
        var intervalTsTicks = (long)intervalHsec * 10 * TimeSpan.TicksPerMillisecond;
        var intervalTs = TimeSpan.FromTicks(intervalTsTicks);
        return Update(totalElapsed, intervalTs);
    }

    public void Reset() =>
        LastUpdate = TimeSpan.Zero;
}