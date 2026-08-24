using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x0A)]
[Context(Context.Super, 0x0A)]
public sealed class ScrollAction(
    IActorList actorList,
    ITiles tiles,
    IBoardUpdater boardUpdater)
    : IAction
{
    public void Act(int index)
    {
        var actor = actorList[index];
        var color = tiles[actor.Location].Color;

        color++;
        if (color > 0x0F)
        {
            color = 0x09;
        }
        tiles[actor.Location].Color = color;
        boardUpdater.UpdateBoard(actor.Location);
    }
}