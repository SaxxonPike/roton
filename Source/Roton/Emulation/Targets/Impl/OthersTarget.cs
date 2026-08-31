using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Targets.Impl;

/// <summary>
/// Represents the "OTHERS" target, which includes all actors except the sender.
/// </summary>
[Context(Context.Original, "OTHERS")]
[Context(Context.Super, "OTHERS")]
internal sealed class OthersTarget(IActorList actors) : ITarget
{
    public bool Execute(int index, ref SearchContext context, ReadOnlySpan<char> term)
    {
        if (context.Index >= actors.Count)
            return false;

        if (context.Index != index)
            return true;

        context.Index++;
        return context.Index < actors.Count;
    }
}