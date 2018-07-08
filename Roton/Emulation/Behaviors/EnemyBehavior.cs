using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Behaviors
{
    public abstract class EnemyBehavior : ElementBehavior
    {
        private readonly IEngine _engine;

        protected EnemyBehavior(IEngine engine)
        {
            _engine = engine;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.Attack(index, location);
        }
    }
}