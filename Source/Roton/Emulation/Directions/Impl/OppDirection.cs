using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Directions.Impl;

[Context(Context.Original, "OPP")]
[Context(Context.Super, "OPP")]
public sealed class OppDirection(IEngineAccessor engine) : IDirection
{
    private IEngine Engine => engine.Instance;

    public Vector Execute(IOopContext context) => 
        -(Engine.Parser.GetDirection(context) ?? Vector.Idle);
}