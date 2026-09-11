using System;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Time(IConfig config) : ITime
{
    public TimeSpan Elapsed { get; private set; }
    
    public void Reset() => 
        Elapsed = TimeSpan.Zero;

    public void Tick()
    {
        var tsFreq = TimeSpan.TicksPerSecond *
                     config.Engine.MasterClockNumerator /
                     config.Engine.MasterClockDenominator;
        Elapsed += TimeSpan.FromTicks(tsFreq);
    }
}