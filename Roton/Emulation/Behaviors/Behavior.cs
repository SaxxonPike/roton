using Roton.Emulation.Actions;
using Roton.Emulation.Draws;
using Roton.Emulation.Interactions;

namespace Roton.Emulation.Behaviors
{
    public class Behavior : IBehavior
    {
        public IAction Action { get; set; }
        public IInteraction Interaction { get; set; }
        public IDraw Draw { get; set; }
        public string Name { get; set; }
    }
}