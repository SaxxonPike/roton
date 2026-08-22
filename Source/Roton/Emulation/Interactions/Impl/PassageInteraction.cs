using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0B)]
[Context(Context.Super, 0x0B)]
public sealed class PassageInteraction(
    IEngineAccessor engine,
    ITiles tiles,
    IActorList actorList,
    IElementList elementList,
    IState state,
    ISounds sounds)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var searchColor = tiles[location].Color;
        var passageIndex = Engine.ActorIndexAt(location);
        var passageTarget = actorList[passageIndex].P3;
        Engine.SetBoard(passageTarget);
        var target = new Location();

        for (var x = 1; x <= tiles.Width; x++)
        {
            for (var y = 1; y <= tiles.Height; y++)
            {
                var loc = new Location(x, y);
                if (tiles[loc].Id == elementList.PassageId && tiles[loc].Color == searchColor)
                    target = new Location(x, y);
            }
        }

        Engine.CleanUpPassageMovement();

        if (target.X != 0)
            actorList.Player.Location = target;

        state.GamePaused = true;
        Engine.PlaySound(4, sounds.Passage);
        Engine.FadePurple();
        Engine.EnterBoard();
        vector = Vector.Idle;
    }
}