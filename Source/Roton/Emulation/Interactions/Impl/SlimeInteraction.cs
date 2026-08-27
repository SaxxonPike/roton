using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x25)]
[Context(Context.Super, 0x25)]
public sealed class SlimeInteraction(
    IEngineAccessor engine,
    ITiles tiles,
    IElementList elementList,
    ISounds sounds,
    ISoundUnit soundUnit,
    IActorList actorList,
    IBoardUpdater boardUpdater)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var color = tiles[location].Color;
        var slimeIndex = actorList.ActorIndexAt(location);
        Engine.Harm(slimeIndex);
        tiles[location] = new Tile(elementList.BreakableId, color);
        boardUpdater.UpdateBoard(location);
        soundUnit.PlaySound(2, sounds.SlimeDie);
    }
}