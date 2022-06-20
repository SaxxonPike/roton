using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0B)]
[Context(Context.Super, 0x0B)]
public sealed class PassageInteraction : IInteraction
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public PassageInteraction(Lazy<IEngine> engine)
    {
        _engine = engine;
    }
        
    public void Interact(IXyPair location, int index, IXyPair vector)
    {
        var searchColor = Engine.Tiles[location].Color;
        var passageIndex = Engine.ActorIndexAt(location);
        var passageTarget = Engine.Actors[passageIndex].P3;
        Engine.SetBoard(passageTarget);
        var target = new Location();

        for (var x = 1; x <= Engine.Tiles.Width; x++)
        {
            for (var y = 1; y <= Engine.Tiles.Height; y++)
            {
                var loc = new Location(x, y);
                if (Engine.Tiles[loc].Id == Engine.ElementList.PassageId && Engine.Tiles[loc].Color == searchColor)
                    target.SetTo(x, y);
            }
        }

        Engine.CleanUpPassageMovement();

        if (target.X != 0)
            Engine.Player.Location.CopyFrom(target);

        Engine.State.GamePaused = true;
        Engine.PlaySound(4, Engine.Sounds.Passage);
        Engine.FadePurple();
        Engine.EnterBoard();
        vector.SetTo(0, 0);
    }
}