using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Traces;

public interface ITraceList
{
    void Trace(TraceFlags flag, IEnumerable<KeyValuePair<string, object?>> data);
}