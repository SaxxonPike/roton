using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x14)]
[Context(Context.Super, 0x14)]
public sealed class ForestInteraction(IEngineAccessor engine) : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        Engine.ClearForest(location);
        Engine.UpdateBoard(location);

        var forestSongLength = Engine.Sounds.Forest.Length;
        var forestIndex = Engine.State.ForestIndex % forestSongLength;
        Engine.State.ForestIndex = (forestIndex + 2) % forestSongLength;
        Engine.PlaySound(3, Engine.Sounds.Forest, forestIndex, 2);

        if (!Engine.Alerts.Forest)
            return;

        Engine.SetMessage(Engine.Facts.LongMessageDuration, Engine.Alerts.ForestMessage);
        Engine.Alerts.Forest = false;
    }
}