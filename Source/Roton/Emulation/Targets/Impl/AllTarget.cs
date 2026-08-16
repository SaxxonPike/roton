using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets.Impl;

[Context(Context.Original, "ALL")]
[Context(Context.Super, "ALL")]
public sealed class AllTarget(IActors actors) : ITarget
{
    private IActors Actors => actors;

    public bool Execute(int index, ISearchContext context, string term)
    {
        return context.SearchIndex < Actors.Count;
    }
}