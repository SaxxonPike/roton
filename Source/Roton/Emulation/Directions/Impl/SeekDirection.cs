using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "SEEK")]
[Context(Context.Super, "SEEK")]
internal sealed class SeekDirection(IEngineAccessor engine) : IDirection
{
    private IEngine Engine => engine.Instance;

    public Vector Execute(ref OopContext context, ref Word instruction)
    {
        return Engine.Seek(context.Actor.Location);
    }
}