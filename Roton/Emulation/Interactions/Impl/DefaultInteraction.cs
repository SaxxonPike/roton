using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class DefaultInteraction : IInteraction
    {
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
        }
    }
}