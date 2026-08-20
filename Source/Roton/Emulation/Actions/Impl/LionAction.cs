using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x29)]
[Context(Context.Super, 0x29)]
public sealed class LionAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        var vector = new Vector();

        vector = actor.P1 >= Engine.Random.GetNext(10)
            ? Engine.Seek(actor.Location)
            : Engine.Rnd();

        var target = actor.Location + vector;
        var element = Engine.Tiles.ElementAt(target);
        if (element.IsFloor)
        {
            Engine.MoveActor(index, target);
        }
        else if (element.Id == Engine.ElementList.PlayerId)
        {
            Engine.Attack(index, target);
        }
    }
}