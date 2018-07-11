using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Zzt)]
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class DefaultInteraction : IInteraction
    {
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
        }
    }
}