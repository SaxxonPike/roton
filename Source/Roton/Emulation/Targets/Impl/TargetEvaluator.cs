using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Targets.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class TargetEvaluator(
    ITargetList targets) 
    : ITargetEvaluator
{
    public bool TryEval(int index, ref SearchContext context, ReadOnlySpan<char> term)
    {
        context.Index++;
        var target = targets.Get(term) ?? targets.Get(string.Empty);
        return target?.Execute(index, ref context, term) ?? false;
    }
}