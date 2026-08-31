using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x1C)]
[Context(Context.Super, 0x1C)]
internal sealed class InvisibleWallInteraction(
    ITiles tiles,
    IElementList elementList,
    IAlerts alerts,
    ISounds sounds,
    IFacts facts,
    ISoundUnit soundUnit,
    IBoardUpdater boardUpdater,
    IMessenger messenger)
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        tiles[location].Id = elementList.NormalId;
        boardUpdater.UpdateBoard(location);
        soundUnit.PlaySound(3, sounds.Invisible);
        messenger.SetMessage(facts.ShortMessageDuration, alerts.InvisibleMessage);
    }
}