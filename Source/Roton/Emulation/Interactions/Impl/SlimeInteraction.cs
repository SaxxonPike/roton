using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x25)]
[Context(Context.Super, 0x25)]
internal sealed class SlimeInteraction(
    IEngineAccessor engine,
    ITiles tiles,
    IElementList elements,
    ISounds sounds,
    ISoundUnit soundUnit,
    IActorList actors,
    IBoardUpdater boardUpdater,
    IAttacker attacker)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var color = tiles[location].Color;
        var slimeIndex = actors.ActorIndexAt(location);
        attacker.Harm(slimeIndex);
        tiles[location] = new Tile(elements.BreakableId, color);
        boardUpdater.UpdateBoard(location);
        soundUnit.PlaySound(2, sounds.SlimeDie);
    }
}