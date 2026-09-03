using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Interactions.Impl;

[Context(Context.Original, 0x14)]
[Context(Context.Super, 0x14)]
internal sealed class ForestInteraction(
    IAlerts alerts,
    IFacts facts,
    ISounds sounds,
    IState state,
    ISoundPlayer soundPlayer,
    IBoardUpdater boardUpdater,
    IMessenger messenger,
    IForestHandler forestHandler)
    : IInteraction
{
    public void Interact(Location location, int index, ref Vector vector)
    {
        forestHandler.ClearForest(location);
        boardUpdater.UpdateBoard(location);

        var forestSongLength = sounds.Forest.Length;
        var forestIndex = state.ForestIndex % forestSongLength;
        state.ForestIndex = (forestIndex + 2) % forestSongLength;
        soundPlayer.PlaySound(3, sounds.Forest.Slice(forestIndex, 2));

        if (!alerts.Forest)
            return;

        messenger.SetMessage(facts.LongMessageDuration, alerts.ForestMessage);
        alerts.Forest = false;
    }
}