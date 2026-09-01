using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x0B)]
[Context(Context.Super, 0x0B)]
internal sealed class PassageInteraction(
    IEngineAccessor engine,
    ITiles tiles,
    IActorList actors,
    IElementList elements,
    IState state,
    ISounds sounds,
    ISoundUnit soundUnit,
    IWorldUnit worldUnit,
    IPlayerUpdater playerUpdater,
    IPlayerEnterHandler playerEnterHandler)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var searchColor = tiles[location].Color;
        var passageIndex = actors.ActorIndexAt(location);
        var passageTarget = actors[passageIndex].P3;
        worldUnit.SetBoard(passageTarget);
        var target = new Location();

        for (var x = 1; x <= tiles.Width; x++)
        {
            for (var y = 1; y <= tiles.Height; y++)
            {
                var loc = new Location(x, y);
                if (tiles[loc].Id == elements.PassageId && tiles[loc].Color == searchColor)
                    target = new Location(x, y);
            }
        }

        playerUpdater.CleanUpPassageMovement();

        if (target.X != 0)
            actors.Player.Location = target;

        state.GamePaused = true;
        soundUnit.PlaySound(4, sounds.Passage);
        Engine.FadePurple();
        playerEnterHandler.EnterBoard();
        vector = Vector.Idle;
    }
}