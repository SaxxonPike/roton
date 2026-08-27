using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x1E)]
[Context(Context.Super, 0x1E)]
public sealed class TransporterInteraction(
    IPusher pusher)
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        pusher.Transport(location - vector, vector);
        vector = Vector.Idle;
    }
}