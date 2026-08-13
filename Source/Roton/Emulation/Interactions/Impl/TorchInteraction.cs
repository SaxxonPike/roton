using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x06)]
public sealed class TorchInteraction : IInteraction
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;
        
    public TorchInteraction(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Interact(IXyPair location, int index, IXyPair vector)
    {
        Engine.World.Torches++;
        Engine.RemoveItem(location);
        Engine.Hud.UpdateStatus();
        if (Engine.Alerts.TorchPickup)
        {
            Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.TorchMessage);
            Engine.Alerts.TorchPickup = false;
        }

        Engine.PlaySound(3, Engine.Sounds.Torch);
    }
}