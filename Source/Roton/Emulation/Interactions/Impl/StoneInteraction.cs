using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Super, 0x40)]
internal sealed class StoneInteraction(
    IWorld world,
    IHud hud,
    IFacts facts,
    IAlerts alerts,
    IMessenger messenger,
    IDestroyer destroyer) 
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        if (world.Stones < 0)
            world.Stones = 0;

        world.Stones++;
        destroyer.Destroy(location);
        hud.UpdateStatus();
        messenger.SetMessage(facts.LongMessageDuration, alerts.StoneMessage);
    }
}