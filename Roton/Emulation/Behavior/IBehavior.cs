using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public interface IBehavior
    {
        string KnownName { get; }
        void Act(int index);
        AnsiChar Draw(IEngine engine, IXyPair location);
        void Interact(IXyPair location, int index, IXyPair vector);
    }
}