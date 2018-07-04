using Roton.Core;

namespace Roton.Emulation.Execution
{
    public class OopContextFactory : IOopContextFactory
    {
        private readonly IActorList _actorList;

        public OopContextFactory(IActorList actorList)
        {
            _actorList = actorList;
        }
        
        public IOopContext Create(int index, IExecutable instructionSource, string name)
        {
            return new OopContext(index, instructionSource, name, _actorList);
        }
    }
}