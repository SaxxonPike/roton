using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "RND")]
[Context(Context.Super, "RND")]
public sealed class RndDirection(IEngineAccessor engine) : IDirection
{
    private IEngine Engine => engine.Instance;

    public Vector Execute(ref OopContext context, ref Word instruction)
    {
        return Engine.Rnd();
    }
}