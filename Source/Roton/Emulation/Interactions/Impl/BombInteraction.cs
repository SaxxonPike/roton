using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0D)]
[Context(Context.Super, 0x0D)]
public sealed class BombInteraction : IInteraction
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public BombInteraction(Lazy<IEngine> engine)
    {
        _engine = engine;
    }
    public void Interact(IXyPair location, int index, IXyPair vector)
    {
        var actor = Engine.ActorAt(location);
        if (actor.P1 == 0)
        {
            actor.P1 = Engine.Facts.BombCountdownStart;
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