using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets.Impl;

[Context(Context.Original, "OTHERS")]
[Context(Context.Super, "OTHERS")]
public sealed class OthersTarget(IActors actors) : ITarget
{
    private IActors Actors => actors;

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