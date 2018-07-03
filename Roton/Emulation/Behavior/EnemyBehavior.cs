using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public abstract class EnemyBehavior : ElementBehavior
    {
        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            engine.Attack(index, location);
        }
    }
}