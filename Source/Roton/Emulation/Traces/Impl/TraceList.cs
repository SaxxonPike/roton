using System.Collections.Generic;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Traces.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public class TraceList : ITraceList
{
    private Dictionary<TraceFlags, List<ITrace>> _tracers = [];

    public void Trace(TraceFlags flag, IEnumerable<KeyValuePair<string, object?>> data)
    {
        throw new System.NotImplementedException();
    }
}