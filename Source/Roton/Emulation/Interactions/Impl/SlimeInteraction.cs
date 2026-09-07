using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x25)]
[Context(Context.Super, 0x25)]
internal sealed class SlimeInteraction(
    ITiles tiles,
    IElementList elements,
    ISounds sounds,
    ISoundPlayer soundPlayer,
    IActorList actors,
    IBoardUpdater boardUpdater,
    IDamager damager)
    : IInteraction
{
    /// <remarks>
    /// RoZ: ElementSlimeTouch
    /// </remarks>
    public void Interact(Location location, int index, ref Vector vector)
    {
        var color = tiles[location].Color;
        var slimeIndex = actors.IndexAt(location);
        damager.Harm(slimeIndex);
        tiles[location] = new Tile(elements.BreakableId, color);
        boardUpdater.UpdateBoard(location);
        soundPlayer.PlaySound(2, sounds.SlimeDie);
    }
}