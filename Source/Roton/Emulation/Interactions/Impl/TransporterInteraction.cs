using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, id: 0x1E)]
[Context(Context.Super, id: 0x1E)]
internal sealed class TransporterInteraction(
    IPusher pusher)
    : IInteraction
{
    /// <remarks>
    /// RoZ: ElementTransporterTouch
    /// </remarks>
    public void Interact(Location location, int index, ref Vector vector)
    {
        pusher.Transport(location - vector, vector);
        vector = Vector.Idle;
    }
}