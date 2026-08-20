using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class DefaultInteraction : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
    }
}