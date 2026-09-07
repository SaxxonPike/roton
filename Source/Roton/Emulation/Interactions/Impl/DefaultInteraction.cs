using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class DefaultInteraction : IInteraction
{
    /// <remarks>
    /// RoZ: ElementDefaultTouch
    /// </remarks>
    public void Interact(Location location, int index, ref Vector vector)
    {
    }
}