using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x0A)]
[Context(Context.Super, 0x0A)]
public sealed class ScrollAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        var color = tiles[actor.Location].Color;

        color++;
        if (color > 0x0F)
        {
            color = 0x09;
        }
        tiles[actor.Location].Color = color;
        Engine.UpdateBoard(actor.Location);
    }
}