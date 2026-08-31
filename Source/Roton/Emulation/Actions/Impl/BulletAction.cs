using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the bullet element.
/// </summary>
[Context(Context.Original, 0x12)]
[Context(Context.Super, 0x45)]
internal sealed class BulletAction(
    IEngineAccessor engine,
    IActorList actors,
    IElementList elements,
    ITiles tiles,
    ISounds sounds,
    IWorld world,
    IState state,
    IFacts facts,
    ISoundUnit soundUnit,
    IHud hud,
    IBroadcaster broadcaster,
    IMover mover)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actors[index];
        var canRicochet = true;
        while (true)
        {
            var target = actor.Location + actor.Vector;
            var element = tiles.ElementAt(target);
            if (element.IsFloor || elements.IsWater(element.Id))
            {
                mover.MoveActor(index, target);
                break;
            }

            if (canRicochet && element.Id == elements.RicochetId)
            {
                canRicochet = false;
                actor.Vector = -actor.Vector;
                soundUnit.PlaySound(1, sounds.Ricochet);
                continue;
            }

            if (element.Id == elements.BreakableId ||
                element.IsDestructible && (element.Id == elements.PlayerId || actor.P1 == 0))
            {
                if (element.Points != 0)
                {
                    world.Score += element.Points;
                    hud.UpdateStatus();
                }

                Engine.Attack(index, target);
                break;
            }

            if (canRicochet &&
                tiles[actor.Location + actor.Vector.Clockwise()].Id == elements.RicochetId)
            {
                canRicochet = false;
                actor.Vector = actor.Vector.CounterClockwise();
                soundUnit.PlaySound(1, sounds.Ricochet);
                continue;
            }

            if (canRicochet &&
                tiles[actor.Location + actor.Vector.CounterClockwise()].Id == elements.RicochetId)
            {
                canRicochet = false;
                actor.Vector = actor.Vector.Clockwise();
                soundUnit.PlaySound(1, sounds.Ricochet);
                continue;
            }

            Engine.RemoveActor(index);
            state.ActIndex--;
            if (element.Id == elements.ObjectId || element.Id == elements.ScrollId)
            {
                broadcaster.BroadcastLabel(-actors.ActorIndexAt(target), facts.ShotLabel, false);
            }

            break;
        }
    }
}