using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the tick action for the slime element.
/// </summary>
[Context(Context.Original, 0x25)]
[Context(Context.Super, 0x25)]
internal sealed class SlimeAction(
    IActorList actors,
    ITiles tiles,
    IElementList elements,
    IState state,
    IBoardUpdater boardUpdater,
    ISpawner spawner,
    IMover mover,
    IActorManager actorManager)
    : IAction
{
    public void Act(int index)
    {
        var actor = actors[index];

        if (actor.P1 >= actor.P2)
        {
            var spawnCount = 0;
            var color = tiles[actor.Location].Color;
            var slimeElement = elements.Slime();
            var slimeTrailTile = new Tile(elements.BreakableId, color);
            var source = actor.Location;

            actor.P1 = 0;

            for (var i = 0; i < 4; i++)
            {
                var target = source + state.GetCardinalVector(i);

                if (!tiles.ElementAt(target).IsFloor)
                    continue;

                if (spawnCount == 0)
                {
                    mover.Move(index, target);
                    tiles[source] = slimeTrailTile;
                    boardUpdater.UpdateBoard(source);
                }
                else
                {
                    spawner.SpawnActor(target, new Tile(elements.SlimeId, color), slimeElement.Cycle, null);
                    actors[state.ActorCount].P2 = actor.P2;
                }

                spawnCount++;
            }

            if (spawnCount == 0)
            {
                actorManager.Free(index);
                tiles[source] = slimeTrailTile;
                boardUpdater.UpdateBoard(source);
            }
        }
        else
        {
            actor.P1++;
        }
    }
}