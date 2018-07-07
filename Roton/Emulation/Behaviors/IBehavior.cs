using Roton.Core;

namespace Roton.Emulation.Behaviors
{
    public interface IBehavior
    {
        string KnownName { get; }
        void Act(int index);
        AnsiChar Draw(IXyPair location);
        void Interact(IXyPair location, int index, IXyPair vector);
    }
}