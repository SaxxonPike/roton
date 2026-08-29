using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Traces.Impl;

public class CrashTrace : ITrace
{
    public TraceFlags SupportedFlags => TraceFlags.Crash;

    public void Trace(TraceFlags flag, Dictionary<string, object?> context)
    {
        throw new System.NotImplementedException();
    }
}