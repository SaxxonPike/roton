using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x0A)]
[Context(Context.Super, 0x0A)]
public sealed class ScrollAction : IAction
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public ScrollAction(Lazy<IEngine> engine)
    {
        _engine = engine;
    }
        
    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        var color = Engine.Tiles[actor.Location].Color;

        color++;
        if (color > 0x0F)
        {
            color = 0x09;
        }
        Engine.Tiles[actor.Location].Color = color;
        Engine.UpdateBoard(actor.Location);
    }
}