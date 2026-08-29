using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x29)]
[Context(Context.Super, 0x29)]
internal sealed class LionAction(
    IEngineAccessor engine,
    IActorList actorList,
    IRandomizer randomizer,
    ITiles tiles,
    IElementList elementList)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];
        var vector = new Vector();

        vector = actor.P1 >= randomizer.GetNext(10)
            ? Engine.Seek(actor.Location)
            : Engine.Rnd();

        var target = actor.Location + vector;
        var element = tiles.ElementAt(target);
        if (element.IsFloor)
        {
            Engine.MoveActor(index, target);
        }
        else if (element.Id == elementList.PlayerId)
        {
            Engine.Attack(index, target);
        }
    }
}