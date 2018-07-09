using Roton.Emulation.Actions;
using Roton.Emulation.Draws;
using Roton.Emulation.Interactions;

namespace Roton.Emulation.Behaviors
{
    public interface IBehavior
    {
        IAction Action { get; }
        IInteraction Interaction { get; }
        IDraw Draw { get; }
        string Name { get; }
    }
}