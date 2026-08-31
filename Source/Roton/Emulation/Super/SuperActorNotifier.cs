using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperActorNotifier(
    IActorList actors)
    : ActorNotifier
{
    public override void NotifyActorSentLabel(int index)
    {
        // When an object receives a label, the current
        // in-progress movement counter is reset.

        actors[index].P2 = 0;
    }
}