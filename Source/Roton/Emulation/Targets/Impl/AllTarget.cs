using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets.Impl;

[Context(Context.Original, "ALL")]
[Context(Context.Super, "ALL")]
public sealed class AllTarget(IActors actors) : ITarget
{
    private IActors Actors => actors;

    public bool Execute(int index, ref SearchContext context, ReadOnlySpan<char> term)
    {
        return context.Index < Actors.Count;
    }
}