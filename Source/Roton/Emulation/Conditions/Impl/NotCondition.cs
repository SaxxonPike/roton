using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "NOT")]
[Context(Context.Super, "NOT")]
public sealed class NotCondition(
    IEngineAccessor engine,
    IParser parser)
    : ICondition
{
    private IEngine Engine => engine.Instance;

    public bool? Execute(ref OopContext context, ref Word instruction) =>
        parser.TryEvalCondition(ref context, ref instruction, out var result)
            ? !result
            : null;
}