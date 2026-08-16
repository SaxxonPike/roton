using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "ANY")]
[Context(Context.Super, "ANY")]
public sealed class AnyCondition(IEngineAccessor engine) : ICondition
{
    private IEngine Engine => engine.Instance;

    public bool? Execute(IOopContext context)
    {
        var kind = Engine.Parser.GetKind(context);
        if (kind == null)
            return null;

        return Engine.FindTile(kind, new Location(0, 1));
    }
}