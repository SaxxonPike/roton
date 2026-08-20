using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x26)]
public sealed class SharkAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];
        var vector = new Vector();

        vector = actor.P1 > Engine.Random.GetNext(10)
            ? Engine.Seek(actor.Location)
            : Engine.Rnd();

        var target = actor.Location + vector;
        var targetElement = Engine.Tiles.ElementAt(target);

        if (targetElement.Id == Engine.Elements.WaterId || targetElement.Id == Engine.Elements.LavaId)
        {
            Engine.MoveActor(index, target);
        }
        else if (targetElement.Id == Engine.Elements.PlayerId)
        {
            Engine.Attack(index, target);
        }
    }
}