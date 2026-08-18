using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "RNDNS")]
[Context(Context.Super, "RNDNS")]
public sealed class RndNsDirection(IEngineAccessor engine) : IDirection
{
    private IEngine Engine => engine.Instance;

    public Vector Execute(ref OopContext context, ref Word instruction)
    {
        return Engine.Random.GetNext(2) == 0
            ? Vector.North
            : Vector.South;
    }
}