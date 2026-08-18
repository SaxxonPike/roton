using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "CW")]
[Context(Context.Super, "CW")]
public sealed class CwDirection(IEngineAccessor engine) : IDirection
{
    private IEngine Engine => engine.Instance;

    public Vector Execute(ref OopContext context, ref Word instruction) => 
        (Engine.Parser.GetDirection(ref context, ref instruction) ?? Vector.Idle).Clockwise();
}