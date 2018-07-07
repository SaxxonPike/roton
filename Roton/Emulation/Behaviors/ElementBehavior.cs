using Roton.Core;

namespace Roton.Emulation.Behaviors
{
    public abstract class ElementBehavior : IBehavior
    {
        public virtual void Act(int index)
        {
        }

        public virtual AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(0x3F, 0x40);
        }

        public virtual void Interact(IXyPair location, int index, IXyPair vector)
        {
        }

        public abstract string KnownName { get; }
    }
}