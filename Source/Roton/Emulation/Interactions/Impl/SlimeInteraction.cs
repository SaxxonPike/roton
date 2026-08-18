using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x25)]
[Context(Context.Super, 0x25)]
public sealed class SlimeInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        var color = Engine.Tiles[location].Color;
        var slimeIndex = Engine.ActorIndexAt(location);
        Engine.Harm(slimeIndex);
        Engine.Tiles[location] = new Tile(Engine.ElementList.BreakableId, color);
        Engine.UpdateBoard(location);
        Engine.PlaySound(2, Engine.Sounds.SlimeDie);
    }
}