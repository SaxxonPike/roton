using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "NOT")]
[Context(Context.Super, "NOT")]
public sealed class NotCondition(IEngineAccessor engine) : ICondition
{
    private IEngine Engine => engine.Instance;

    public bool? Execute(ref OopContext context, ref Word instruction) =>
        Engine.Parser.TryEvalCondition(ref context, ref instruction, out var result)
            ? !result
            : null;
}