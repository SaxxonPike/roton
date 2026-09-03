using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x13)]
[Context(Context.Super, 0x13)]
internal sealed class WaterInteraction(
    ISounds sounds,
    IAlerts alerts,
    IFacts facts,
    IConfig config,
    ISoundPlayer soundPlayer,
    IMessenger messenger)
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        if (config.NoPesterMode)
            return;

        soundPlayer.PlaySound(3, sounds.Water);
        messenger.SetMessage(facts.ShortMessageDuration, alerts.WaterMessage);
    }
}