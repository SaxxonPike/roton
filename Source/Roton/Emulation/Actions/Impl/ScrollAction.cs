using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, id: 0x0A)]
[Context(Context.Super, id: 0x0A)]
internal sealed class ScrollAction(
    IActorList actors,
    ITiles tiles,
    IBoardUpdater boardUpdater)
    : IAction
{
    /// <remarks>
    /// RoZ: ElementScrollTick
    /// </remarks>
    public void Act(int index)
    {
        var actor = actors[index];
        var color = tiles[actor.Location].Color;

        color++;
        if (color > 0x0F)
            color = 0x09;

        tiles[actor.Location].Color = color;
        boardUpdater.UpdateBoard(actor.Location);
    }
}