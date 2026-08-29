using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x13)]
[Context(Context.Super, 0x13)]
internal sealed class WaterInteraction(
    IEngineAccessor engine,
    ISounds sounds,
    IAlerts alerts,
    IFacts facts,
    IConfig config,
    ISoundUnit soundUnit)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        if (config.NoPesterMode)
            return;

        soundUnit.PlaySound(3, sounds.Water);
        Engine.SetMessage(facts.ShortMessageDuration, alerts.WaterMessage);
    }
}