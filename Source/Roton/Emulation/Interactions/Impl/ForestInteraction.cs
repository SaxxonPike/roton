using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x14)]
[Context(Context.Super, 0x14)]
public sealed class ForestInteraction(
    IEngineAccessor engine,
    IAlerts alerts,
    IFacts facts,
    ISounds sounds,
    IState state,
    ISoundUnit soundUnit)
    : IInteraction
{
    private IEngine Engine => engine.Instance;

    public void Interact(Location location, int index, ref Vector vector)
    {
        Engine.ClearForest(location);
        Engine.UpdateBoard(location);

        var forestSongLength = sounds.Forest.Length;
        var forestIndex = state.ForestIndex % forestSongLength;
        state.ForestIndex = (forestIndex + 2) % forestSongLength;
        soundUnit.PlaySound(3, sounds.Forest, forestIndex, 2);

        if (!alerts.Forest)
            return;

        Engine.SetMessage(facts.LongMessageDuration, alerts.ForestMessage);
        alerts.Forest = false;
    }
}