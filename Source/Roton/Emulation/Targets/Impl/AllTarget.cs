using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Targets.Impl;

[Context(Context.Original, "ALL")]
[Context(Context.Super, "ALL")]
public sealed class AllTarget(IActorList actors) : ITarget
{
    private IActorList Actors => actors;

    public bool Execute(int index, ref SearchContext context, ReadOnlySpan<char> term)
    {
        return context.Index < Actors.Count;
    }
}