using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Super, 0x40)]
public sealed class StoneAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        Engine.Tiles[actor.Location].Color =
            (Engine.Tiles[actor.Location].Color & 0x70) + Engine.Random.GetNext(7) + 9;
        Engine.UpdateBoard(Engine.Actors[index].Location);
    }
}