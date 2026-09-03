using System;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Delayer(
    IScheduler scheduler)
    : IDelayer
{
    public void Delay(int msec)
    {
        var waitUntil = DateTime.Now + TimeSpan.FromMilliseconds(msec);
        while (DateTime.Now < waitUntil)
            scheduler.WaitForTick();
    }
}