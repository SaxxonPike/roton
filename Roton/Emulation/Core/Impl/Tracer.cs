using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class Tracer : ITracer
    {
        public void Trace(IOopContext oopContext)
        {
            // Default implementation does nothing for now..
        }
    }
}