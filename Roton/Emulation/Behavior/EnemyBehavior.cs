using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public abstract class EnemyBehavior : ElementBehavior
    {
        private readonly IEngine _engine;

        public EnemyBehavior(IEngine engine)
        {
            _engine = engine;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.Attack(index, location);
        }
    }
}