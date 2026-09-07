using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class ConditionEvaluator(
    IParser parser,
    IConditionList conditions,
    IFlags flags)
    : IConditionEvaluator
{
    public bool TryEval(ref OopContext context, ref Word instruction, out bool result)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var name = parser.ReadWord(context.Index, ref instruction, buffer);

        if (name.IsEmpty)
        {
            result = false;
            return false;
        }

        var condition = conditions.Get(name);
        result = condition?.Execute(ref context, ref instruction) ?? flags.Contains(name);
        return true;
    }
}