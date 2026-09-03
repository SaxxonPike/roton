using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperActorNotifier(
    IActorList actors)
    : IActorNotifier
{
    public void NotifyLabelTaken(int index)
    {
        // When an object receives a label, the current
        // in-progress movement counter is reset.

        actors[index].P2 = 0;
    }
}