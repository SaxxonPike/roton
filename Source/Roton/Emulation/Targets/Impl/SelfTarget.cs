using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Targets.Impl;

/// <summary>
/// Represents the "SELF" target, which includes only the sender.
/// </summary>
[Context(Context.Original, "SELF")]
[Context(Context.Super, "SELF")]
internal sealed class SelfTarget : ITarget
{
    public bool Execute(int index, ref SearchContext context, ReadOnlySpan<char> term)
    {
        if (index <= 0)
            return false;

        if (context.Index > index)
            return false;

        context.Index = index;
        return true;
    }
}