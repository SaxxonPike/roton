using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "RNDNS")]
[Context(Context.Super, "RNDNS")]
public sealed class RndNsDirection(IRandomizer randomizer) : IDirection
{
    public Vector Execute(ref OopContext context, ref Word instruction)
    {
        return randomizer.GetNext(2) == 0
            ? Vector.North
            : Vector.South;
    }
}