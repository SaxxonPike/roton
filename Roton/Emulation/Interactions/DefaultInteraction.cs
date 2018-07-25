using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class DefaultInteraction : IInteraction
    {
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
        }
    }
}