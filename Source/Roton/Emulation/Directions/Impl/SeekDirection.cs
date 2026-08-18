using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "SEEK")]
[Context(Context.Super, "SEEK")]
public sealed class SeekDirection(IEngineAccessor engine) : IDirection
{
    private IEngine Engine => engine.Instance;

    public Vector Execute(IOopContext context)
    {
        return Engine.Seek(context.Actor.Location);
    }
}