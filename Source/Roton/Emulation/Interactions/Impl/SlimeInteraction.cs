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
    ISoundUnit soundUnit)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var color = tiles[location].Color;
        var slimeIndex = Engine.ActorIndexAt(location);
        Engine.Harm(slimeIndex);
        tiles[location] = new Tile(elementList.BreakableId, color);
        Engine.UpdateBoard(location);
        soundUnit.PlaySound(2, sounds.SlimeDie);
    }
}