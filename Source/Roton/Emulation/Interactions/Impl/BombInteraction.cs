using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0D)]
[Context(Context.Super, 0x0D)]
public sealed class BombInteraction(
    IEngineAccessor engine,
    IFacts facts,
    IAlerts alerts,
    ISounds sounds,
    ISoundUnit soundUnit)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var actor = Engine.ActorAt(location);
        if (actor.P1 == 0)
        {
            actor.P1 = (byte)facts.BombCountdownStart;
            Engine.UpdateBoard(location);
            Engine.SetMessage(facts.LongMessageDuration, alerts.BombMessage);
            soundUnit.PlaySound(4, sounds.BombActivate);
        }
        else
        {
            Engine.Push(location, vector);
        }
    }
}