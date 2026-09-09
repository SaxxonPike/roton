using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, id: 0x1C)]
[Context(Context.Super, id: 0x1C)]
internal sealed class InvisibleWallInteraction(
    ITiles tiles,
    IElementList elements,
    IAlerts alerts,
    ISounds sounds,
    IFacts facts,
    ISoundPlayer soundPlayer,
    IBoardUpdater boardUpdater,
    IMessenger messenger)
    : IInteraction
{
    /// <remarks>
    /// RoZ: ElementInvisibleTouch
    /// </remarks>
    public void Interact(Location location, int index, ref Vector vector)
    {
        tiles[location].Id = elements.NormalId;
        boardUpdater.UpdateBoard(location);
        soundPlayer.PlaySound(3, sounds.Invisible);
        messenger.SetMessage(facts.ShortMessageDuration, alerts.InvisibleMessage);
    }
}