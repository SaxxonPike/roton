using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class Tracer : ITracer
{
    public void TraceInput(EngineKeyCode keyCode)
    {
    }

    public void TraceOop(IOopContext oopContext)
    {
    }

    public void TraceStep()
    {
    }

    public bool Enabled { get; set; }
}