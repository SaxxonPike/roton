using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Traces.Impl;

public class OopTrace : ITrace
{
    public TraceFlags SupportedFlags => TraceFlags.Oop;

    public void Trace(TraceFlags flag, Dictionary<string, object?> context)
    {
        throw new System.NotImplementedException();
    }
}