using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Targets.Impl;

/// <summary>
/// Represents the "ALL" target, which includes all actors including the sender.
/// </summary>
[Context(Context.Original, "ALL")]
[Context(Context.Super, "ALL")]
internal sealed class AllTarget(IActorList actors) : ITarget
{
    public bool Execute(int index, ref SearchContext context, ReadOnlySpan<char> term) => 
        context.Index < actors.Count;
}