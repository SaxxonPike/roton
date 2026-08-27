using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperActorNotifier(
    IActorList actorList)
    : ActorNotifier
{
    public override void NotifyActorSentLabel(int index)
    {
        // When an object receives a label, the current
        // in-progress movement counter is reset.

        actorList[index].P2 = 0;
    }
}