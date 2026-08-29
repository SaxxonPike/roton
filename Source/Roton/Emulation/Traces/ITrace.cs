using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Traces;

public interface ITrace
{
    TraceFlags SupportedFlags { get; }
    void Trace(TraceFlags flag, Dictionary<string, object?> context);
}