using Roton.Core;

namespace Roton.Emulation.Behaviors
{
    public abstract class EnemyBehavior : ElementBehavior
    {
        private readonly IMover _mover;

        protected EnemyBehavior(IMover mover)
        {
            _mover = mover;
        }
        
        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _mover.Attack(index, location);
        }
    }
}