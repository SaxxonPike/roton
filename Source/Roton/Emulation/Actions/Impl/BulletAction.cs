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
    IActorList actorList,
    IElementList elementList,
    ITiles tiles,
    ISounds sounds,
    IWorld world,
    IState state,
    IFacts facts,
    ISoundUnit soundUnit,
    IHud hud,
    IBroadcaster broadcaster)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];
        var canRicochet = true;
        while (true)
        {
            var target = actor.Location + actor.Vector;
            var element = tiles.ElementAt(target);
            if (element.IsFloor || element.Id == elementList.WaterId || element.Id == elementList.LavaId)
            {
                Engine.MoveActor(index, target);
                break;
            }

            if (canRicochet && element.Id == elementList.RicochetId)
            {
                canRicochet = false;
                actor.Vector = -actor.Vector;
                soundUnit.PlaySound(1, sounds.Ricochet);
                continue;
            }

            if (element.Id == elementList.BreakableId ||
                element.IsDestructible && (element.Id == elementList.PlayerId || actor.P1 == 0))
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
                tiles[actor.Location + actor.Vector.Clockwise()].Id == elementList.RicochetId)
            {
                canRicochet = false;
                actor.Vector = actor.Vector.CounterClockwise();
                soundUnit.PlaySound(1, sounds.Ricochet);
                continue;
            }

            if (canRicochet &&
                tiles[actor.Location + actor.Vector.CounterClockwise()].Id == elementList.RicochetId)
            {
                canRicochet = false;
                actor.Vector = actor.Vector.Clockwise();
                soundUnit.PlaySound(1, sounds.Ricochet);
                continue;
            }

            Engine.RemoveActor(index);
            state.ActIndex--;
            if (element.Id == elementList.ObjectId || element.Id == elementList.ScrollId)
            {
                broadcaster.BroadcastLabel(-actorList.ActorIndexAt(target), facts.ShotLabel, false);
            }

            break;
        }
    }
}