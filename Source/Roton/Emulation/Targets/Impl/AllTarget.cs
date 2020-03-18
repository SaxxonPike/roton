using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets.Impl
{
    [Context(Context.Original, "ALL")]
    [Context(Context.Super, "ALL")]
    public sealed class AllTarget : ITarget
    {
        private readonly Lazy<IActors> _actors;
        private IActors Actors => _actors.Value;

        public AllTarget(Lazy<IActors> actors)
        {
            _actors = actors;
        }

        public bool Execute(int index, ISearchContext context, string term)
        {
            return context.SearchIndex < Actors.Count;
        }
    }
}