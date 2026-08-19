using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "ANY")]
[Context(Context.Super, "ANY")]
public sealed class AnyCondition(IEngineAccessor engine) : ICondition
{
    private IEngine Engine => engine.Instance;

    public bool? Execute(ref OopContext context, ref Word instruction)
    {
        if (!Engine.Parser.TryEvalKind(ref context, ref instruction, out var val))
            return null;

        return Engine.FindTile(val, new Location(0, 1));
    }
}