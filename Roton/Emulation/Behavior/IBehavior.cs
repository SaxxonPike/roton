using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public interface IBehavior
    {
        string KnownName { get; }
        void Act(IEngine engine, int index);
        AnsiChar Draw(IEngine engine, IXyPair location);
        void Interact(IEngine engine, IXyPair location, int index, IXyPair vector);
    }
}