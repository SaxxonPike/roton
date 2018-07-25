using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets
{
    [ContextEngine(ContextEngine.Original, "OTHERS")]
    [ContextEngine(ContextEngine.Super, "OTHERS")]
    public sealed class OthersTarget : ITarget
    {
        private readonly IActors _actors;

        public OthersTarget(IActors actors)
        {
            _actors = actors;
        }

        public bool Execute(ISearchContext context)
        {
            if (context.SearchIndex >= _actors.Count)
                return false;

            if (context.SearchIndex == context.SearchOffset)
                context.SearchIndex++;

            return context.SearchIndex < _actors.Count;
        }
    }
}