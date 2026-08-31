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
    IActorList actors,
    IRandomizer randomizer,
    ITiles tiles,
    IElementList elements,
    IMover mover)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actors[index];

        var vector = actor.P1 > randomizer.GetNext(10)
            ? Engine.Seek(actor.Location)
            : Engine.Rnd();

        var target = actor.Location + vector;
        var targetElement = tiles.ElementAt(target);

        if (elements.IsWater(targetElement.Id))
            mover.MoveActor(index, target);
        else if (targetElement.Id == elements.PlayerId)
            Engine.Attack(index, target);
    }
}