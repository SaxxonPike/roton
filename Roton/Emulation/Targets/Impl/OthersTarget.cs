using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets.Impl
{
    [Context(Context.Original, "OTHERS")]
    [Context(Context.Super, "OTHERS")]
    public sealed class OthersTarget : ITarget
    {
        private readonly Lazy<IActors> _actors;
        private IActors Actors => _actors.Value;

        public OthersTarget(Lazy<IActors> actors)
        {
            _actors = actors;
        }

        public bool Execute(ISearchContext context)
        {
            if (context.SearchIndex >= Actors.Count)
                return false;

            if (context.SearchIndex == context.SearchOffset)
                context.SearchIndex++;

            return context.SearchIndex < Actors.Count;
        }
    }
}