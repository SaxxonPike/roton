using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Traces.Impl;

public class ErrorTrace : ITrace
{
    public TraceFlags SupportedFlags => TraceFlags.Error;

    public void Trace(TraceFlags flag, Dictionary<string, object?> context)
    {
        throw new System.NotImplementedException();
    }
}