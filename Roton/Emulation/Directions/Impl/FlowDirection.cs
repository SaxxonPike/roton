using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl
{
    [Context(Context.Original, "FLOW")]
    [Context(Context.Super, "FLOW")]
    public sealed class FlowDirection : IDirection
    {
        public IXyPair Execute(IOopContext context)
        {
            return context.Actor.Vector.Clone();
        }
    }
}