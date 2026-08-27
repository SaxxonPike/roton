using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x1C)]
[Context(Context.Super, 0x1C)]
public sealed class InvisibleWallInteraction(
    IEngineAccessor engine,
    ITiles tiles,
    IElementList elementList,
    IAlerts alerts,
    ISounds sounds,
    IFacts facts,
    ISoundUnit soundUnit,
    IBoardUpdater boardUpdater)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        tiles[location].Id = elementList.NormalId;
        boardUpdater.UpdateBoard(location);
        soundUnit.PlaySound(3, sounds.Invisible);
        Engine.SetMessage(facts.ShortMessageDuration, alerts.InvisibleMessage);
    }
}