using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public abstract class ElementBehavior : IBehavior
    {
        public virtual void Act(IEngine engine, int index)
        {
        }

        public virtual AnsiChar Draw(IEngine engine, IXyPair location)
        {
            return new AnsiChar(0x3F, 0x40);
        }

        public virtual void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
        }

        public abstract string KnownName { get; }
    }
}