using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original, "NOT")]
[Context(Context.Super, "NOT")]
internal sealed class NotCondition(
    IParser parser)
    : ICondition
{
    public bool? Execute(ref OopContext context, ref Word instruction) =>
        parser.TryEvalCondition(ref context, ref instruction, out var result)
            ? !result
            : null;
}