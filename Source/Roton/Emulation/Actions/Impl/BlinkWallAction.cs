using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the blink wall element.
/// </summary>
[Context(Context.Original, 0x1D)]
[Context(Context.Super, 0x1D)]
internal sealed class BlinkWallAction(
    IEngineAccessor engine,
    ITiles tiles,
    IElementList elements,
    IActorList actors,
    IWorld world,
    IBoardUpdater boardUpdater,
    ITracer tracer,
    IMover mover)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actors[index];

        if (actor.P3 == 0)
            actor.P3 = unchecked((byte)(actor.P1 + 1));

        if (actor.P3 == 1)
        {
            actor.P3 = unchecked((byte)(actor.P2 * 2 + 1));

            var erasedRay = false;
            var target = actor.Location + actor.Vector;
            var emptyElement = elements.EmptyId;

            var rayElement = actor.Vector.X == 0
                ? elements.BlinkRayVId
                : elements.BlinkRayHId;

            var color = tiles[actor.Location].Color;
            var rayTile = new Tile(rayElement, color);

            while (tiles[target] == rayTile)
            {
                tiles[target].Id = emptyElement;
                boardUpdater.UpdateBoard(target);
                target += actor.Vector;
                erasedRay = true;
            }

            if (erasedRay) return;
            var blocked = false;

            do
            {
                if (tiles.ElementAt(target).IsDestructible)
                {
                    Engine.Destroy(target);
                }

                if (tiles[target].Id == elements.PlayerId)
                {
                    var playerIndex = actors.ActorIndexAt(target);
                    Vector testVector;

                    if (actor.Vector.Y == 0)
                    {
                        testVector = new Vector(0, 1);
                        if (tiles[target - testVector].Id == emptyElement)
                        {
                            mover.MoveActor(playerIndex, target - testVector);
                        }
                        else if (tiles[target + testVector].Id == emptyElement)
                        {
                            mover.MoveActor(playerIndex, target + testVector);
                        }
                    }
                    else
                    {
                        testVector = new Vector(1, 0);
                        if (tiles[target + testVector].Id == emptyElement)
                        {
                            mover.MoveActor(playerIndex, target + testVector);
                        }
                        else if (tiles[target - testVector].Id == emptyElement)
                        {
                            // "sum" is not a mistake; this is an original engine bug
                            mover.MoveActor(playerIndex, target + testVector);
                        }
                    }

                    if (tiles[target].Id == elements.PlayerId)
                    {
                        if (playerIndex != 0)
                        {
                            // Ordinarily there is a hang if the player index
                            // is anything but zero. We prevent that here.

                            tracer.TraceCrash("Blink wall hit a trapped player clone");
                        }
                        else
                        {
                            while (world.Health > 0)
                            {
                                Engine.Harm(playerIndex);
                            }
                        }

                        blocked = true;
                    }
                }

                if (tiles[target].Id == emptyElement)
                {
                    tiles[target] = rayTile;
                    boardUpdater.UpdateBoard(target);
                }
                else
                {
                    blocked = true;
                }

                target += actor.Vector;
            } while (!blocked);
        }
        else
        {
            actor.P3--;
        }
    }
}