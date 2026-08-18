using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0B)]
[Context(Context.Super, 0x0B)]
public sealed class PassageInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
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
                    target = new Location(x, y);
            }
        }

        Engine.CleanUpPassageMovement();

        if (target.X != 0)
            Engine.Player.Location = target;

        Engine.State.GamePaused = true;
        Engine.PlaySound(4, Engine.Sounds.Passage);
        Engine.FadePurple();
        Engine.EnterBoard();
        vector = Vector.Idle;
    }
}