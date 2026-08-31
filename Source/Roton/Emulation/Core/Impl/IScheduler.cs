using System;

namespace Roton.Emulation.Core.Impl;

public interface IScheduler
{
    event EventHandler Tick;
    void Advance();
    void Reset();
    void WaitForTick();
}