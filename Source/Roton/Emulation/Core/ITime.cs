using System;

namespace Roton.Emulation.Core;

public interface ITime
{
    TimeSpan Elapsed { get; }
    void Reset();
    void Tick();
}