using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x25)]
[Context(Context.Super, 0x25)]
public sealed class SlimeAction(
    IEngineAccessor engine,
    IActorList actorList,
    ITiles tiles,
    IElementList elementList,
    IState state)
    : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = actorList[index];

        if (actor.P1 >= actor.P2)
        {
            var spawnCount = 0;
            var color = tiles[actor.Location].Color;
            var slimeElement = elementList.Slime();
            var slimeTrailTile = new Tile(elementList.BreakableId, color);
            var source = actor.Location;
            actor.P1 = 0;

            for (var i = 0; i < 4; i++)
            {
                var target = source + Engine.GetCardinalVector(i);
                if (tiles.ElementAt(target).IsFloor)
                {
                    if (spawnCount == 0)
                    {
                        Engine.MoveActor(index, target);
                        tiles[source] = slimeTrailTile;
                        Engine.UpdateBoard(source);
                    }
                    else
                    {
                        Engine.SpawnActor(target, new Tile(elementList.SlimeId, color), slimeElement.Cycle, null);
                        actorList[state.ActorCount].P2 = actor.P2;
                    }

                    spawnCount++;
                }
            }

            if (spawnCount == 0)
            {
                Engine.RemoveActor(index);
                tiles[source] = slimeTrailTile;
                Engine.UpdateBoard(source);
            }
        }
        else
        {
            actor.P1++;
        }
    }
}