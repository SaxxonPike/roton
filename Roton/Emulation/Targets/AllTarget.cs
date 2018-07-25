using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets
{
    [ContextEngine(ContextEngine.Original, "ALL")]
    [ContextEngine(ContextEngine.Super, "ALL")]
    public sealed class AllTarget : ITarget
    {
        private readonly IActors _actors;

        public AllTarget(IActors actors)
        {
            _actors = actors;
        }

        public bool Execute(ISearchContext context)
        {
            return context.SearchIndex < _actors.Count;
        }
    }
}