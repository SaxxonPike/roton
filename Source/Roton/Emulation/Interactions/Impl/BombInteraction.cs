using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0D)]
[Context(Context.Super, 0x0D)]
public sealed class BombInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var actor = Engine.ActorAt(location);
        if (actor.P1 == 0)
        {
            actor.P1 = (byte)Engine.Facts.BombCountdownStart;
            Engine.UpdateBoard(location);
            Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.BombMessage);
            Engine.PlaySound(4, Engine.Sounds.BombActivate);
        }
        else
        {
            Engine.Push(location, vector);
        }
    }
}