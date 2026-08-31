using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Messenger(
    IEngineAccessor engine,
    IActorList actors,
    IHud hud,
    ISpawner spawner,
    IState state,
    IElementList elements) 
    : IMessenger
{
    private IEngine Engine => engine.Instance;
    
    public void SetMessage(int duration, IMessage message)
    {
        var index = actors.ActorIndexAt(new Location(0, 0));
        if (index >= 0)
        {
            Engine.RemoveActor(index);
            hud.UpdateBorder();
        }

        var topMessage = message.Text[0];
        var bottomMessage = message.Text.Count > 1 ? message.Text[1] : string.Empty;

        spawner.SpawnActor(new Location(0, 0), new Tile(elements.MessengerId, 0), 1, state.DefaultActor);
        actors[state.ActorCount].P2 = duration / ((int)state.GameWaitTime + 1);
        state.Message = topMessage;
        state.Message2 = bottomMessage;
    }

}