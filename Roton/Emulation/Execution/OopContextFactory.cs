using Roton.Core;

namespace Roton.Emulation.Execution
{
    public class OopContextFactory : IOopContextFactory
    {
        private readonly IActors _actors;

        public OopContextFactory(IActors actors)
        {
            _actors = actors;
        }
        
        public IOopContext Create(int index, IExecutable instructionSource, string name)
        {
            return new OopContext(index, instructionSource, name, _actors);
        }
    }
}