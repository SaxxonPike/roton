using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x07)]
[Context(Context.Super, 0x07)]
internal sealed class GemInteraction(
    IWorld world,
    IFacts facts,
    IHud hud,
    ISounds sounds,
    IAlerts alerts,
    ISoundUnit soundUnit,
    IFeatures features,
    IMessenger messenger)
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        world.Health += facts.HealthPerGem;
        world.Gems += 1;
        world.Score += facts.ScorePerGem;
        features.RemoveItem(location);
        hud.UpdateStatus();
        soundUnit.PlaySound(2, sounds.Gem);

        if (!alerts.GemPickup)
            return;

        messenger.SetMessage(facts.LongMessageDuration, alerts.GemMessage);
        alerts.GemPickup = false;
    }
}