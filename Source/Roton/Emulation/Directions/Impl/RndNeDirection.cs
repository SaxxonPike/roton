using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "RNDNE")]
[Context(Context.Super, "RNDNE")]
public sealed class RndNeDirection(IEngineAccessor engine) : IDirection
{
    private IEngine Engine => engine.Instance;

    public Vector Execute(ref OopContext context, ref Word instruction)
    {
        return Engine.Random.GetNext(2) == 0
            ? Vector.North
            : Vector.East;
    }
}