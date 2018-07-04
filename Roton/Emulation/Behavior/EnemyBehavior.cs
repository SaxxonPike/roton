using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public abstract class EnemyBehavior : ElementBehavior
    {
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            engine.Attack(index, location);
        }
    }
}