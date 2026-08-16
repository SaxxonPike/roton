using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0E)]
[Context(Context.Super, 0x0E)]
public sealed class EnergizerInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(IXyPair location, int index, IXyPair vector)
    {
        Engine.PlaySound(9, Engine.Sounds.Energizer);
        Engine.RemoveItem(location);
        Engine.World.EnergyCycles = Engine.Facts.EnergyCyclesPerEnergizer;
        Engine.Hud.UpdateStatus();

        if (Engine.Alerts.EnergizerPickup)
        {
            Engine.Alerts.EnergizerPickup = false;
            Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.EnergizerMessage);
        }

        Engine.BroadcastLabel(0, Engine.Facts.EnergizeLabel, false);
    }
}