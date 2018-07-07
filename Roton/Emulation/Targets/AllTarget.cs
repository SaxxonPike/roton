using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.Targets
{
    public class AllTarget : ITarget
    {
        private readonly IActors _actors;

        public AllTarget(IActors actors)
        {
            _actors = actors;
        }
        
        public string Name => "ALL";
        
        public bool Execute(ISearchContext context)
        {
            return context.SearchIndex < _actors.Count;
        }
    }
}