using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "CW")]
[Context(Context.Super, "CW")]
public sealed class CwDirection(IEngineAccessor engine) : IDirection
{
    private IEngine Engine => engine.Instance;

    public Vector Execute(ref OopContext context, ref Word instruction) => 
        Engine.Parser.TryEvalDirection(ref context, ref instruction, out var vec)
            ? vec.Clockwise()
            : Vector.Idle;
}