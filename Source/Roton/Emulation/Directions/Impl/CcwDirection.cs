using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "CCW")]
[Context(Context.Super, "CCW")]
public sealed class CcwDirection(IEngineAccessor engine) : IDirection
{
    private IEngine Engine => engine.Instance;

    public Vector Execute(ref OopContext context, ref Word instruction) =>
        Engine.Parser.TryEvalDirection(ref context, ref instruction, out var vec)
            ? vec.CounterClockwise()
            : Vector.Idle;
}