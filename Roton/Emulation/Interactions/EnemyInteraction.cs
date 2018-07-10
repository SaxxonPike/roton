using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Interactions
{
    public class EnemyInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public EnemyInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.Attack(index, location);
        }
    }
}