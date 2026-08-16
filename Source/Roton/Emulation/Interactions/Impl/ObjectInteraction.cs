using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x24)]
[Context(Context.Super, 0x24)]
public sealed class ObjectInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(IXyPair location, int index, IXyPair vector)
    {
        var objectIndex = Engine.ActorIndexAt(location);
        Engine.BroadcastLabel(-objectIndex, Engine.Facts.TouchLabel, false);
    }
}