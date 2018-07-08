using Roton.Emulation.Data;

namespace Roton.Emulation.Targets
{
    public class OthersTarget : ITarget
    {
        private readonly IActors _actors;

        public OthersTarget(IActors actors)
        {
            _actors = actors;
        }
        
        public string Name => "OTHERS";
        
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