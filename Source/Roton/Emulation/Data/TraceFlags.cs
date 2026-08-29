using System;

namespace Roton.Emulation.Data;

/// <summary>
/// Indicate which items that will be reported to the tracer.
/// </summary>
[Flags]
public enum TraceFlags
{
    Oop = 1 << 0,
    Crash = 1 << 1,
    Broadcast = 1 << 2,
    Error = 1 << 3,
    Step = 1 << 4
}