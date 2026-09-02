using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the lion element.
/// </summary>
[Context(Context.Original, 0x29)]
[Context(Context.Super, 0x29)]
internal sealed class LionAction(
    IActorList actors,
    IRandomizer randomizer,
    ITiles tiles,
    IElementList elements,
    IMover mover,
    INavigator navigator,
    IAttacker attacker)
    : IAction
{
    public void Act(int index)
    {
        var actor = actors[index];

        var vector = actor.P1 >= randomizer.GetNext(10)
            ? navigator.Seek(actor.Location)
            : navigator.Rnd();

        var target = actor.Location + vector;
        var element = tiles.ElementAt(target);
        if (element.IsFloor)
        {
            mover.MoveActor(index, target);
        }
        else if (element.Id == elements.PlayerId)
        {
            attacker.Attack(index, target);
        }
    }
}