using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Targets.Impl;

[Context(Context.Original, "OTHERS")]
[Context(Context.Super, "OTHERS")]
internal sealed class OthersTarget(IActorList actors) : ITarget
{
    private IActorList Actors => actors;

    public bool Execute(int index, ref SearchContext context, ReadOnlySpan<char> term)
    {
        if (context.Index >= Actors.Count)
            return false;

        if (context.Index != index)
            return true;

        context.Index++;
        return context.Index < Actors.Count;
    }
}