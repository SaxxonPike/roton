using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Traces.Impl;

public class BroadcastTrace : ITrace
{
    public TraceFlags SupportedFlags => TraceFlags.Broadcast;

    public void Trace(TraceFlags flag, Dictionary<string, object?> context)
    {
        throw new System.NotImplementedException();
    }
}