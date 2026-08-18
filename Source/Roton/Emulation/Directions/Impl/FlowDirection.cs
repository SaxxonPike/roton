using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "FLOW")]
[Context(Context.Super, "FLOW")]
public sealed class FlowDirection : IDirection
{
    public Vector Execute(ref OopContext context, ref Word instruction)
    {
        return context.Actor.Vector;
    }
}