using System.Collections.Generic;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Messenger(
    IActorList actors,
    IHud hud,
    ISpawner spawner,
    IState state,
    IElementList elements,
    IActorManager actorManager) 
    : IMessenger
{
    public void SetMessage(int duration, IReadOnlyList<string> message)
    {
        var index = actors.IndexAt(new Location(0, 0));
        if (index >= 0)
        {
            actorManager.Free(index);
            hud.UpdateBorder();
        }

        var topMessage = message.Count > 0 ? message[0] : string.Empty;
        var bottomMessage = message.Count > 1 ? message[1] : string.Empty;

        spawner.SpawnActor(new Location(0, 0), new Tile(elements.MessengerId, 0), 1, state.DefaultActor);
        actors[state.ActorCount].P2 = duration / ((int)state.GameWaitTime + 1);
        state.Message = topMessage;
        state.Message2 = bottomMessage;
    }

}