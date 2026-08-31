using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the shark element.
/// </summary>
[Context(Context.Original, 0x26)]
internal sealed class SharkAction(
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

        var vector = actor.P1 > randomizer.GetNext(10)
            ? Engine.Seek(actor.Location)
            : Engine.Rnd();

        var target = actor.Location + vector;
        var targetElement = tiles.ElementAt(target);

        if (targetElement.Id == elementList.WaterId)
            Engine.MoveActor(index, target);
        else if (targetElement.Id == elementList.PlayerId)
            Engine.Attack(index, target);
    }
}