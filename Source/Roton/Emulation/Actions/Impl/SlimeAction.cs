using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original, 0x25)]
[Context(Context.Super, 0x25)]
public sealed class SlimeAction(IEngineAccessor engine) : IAction
{
    private IEngine Engine => engine.Instance;

    public void Act(int index)
    {
        var actor = Engine.Actors[index];

        if (actor.P1 >= actor.P2)
        {
            var spawnCount = 0;
            var color = Engine.Tiles[actor.Location].Color;
            var slimeElement = Engine.Elements.Slime();
            var slimeTrailTile = new Tile(Engine.Elements.BreakableId, color);
            var source = actor.Location;
            actor.P1 = 0;

            for (var i = 0; i < 4; i++)
            {
                var target = source + Engine.GetCardinalVector(i);
                if (Engine.Tiles.ElementAt(target).IsFloor)
                {
                    if (spawnCount == 0)
                    {
                        Engine.MoveActor(index, target);
                        Engine.Tiles[source] = slimeTrailTile;
                        Engine.UpdateBoard(source);
                    }
                    else
                    {
                        Engine.SpawnActor(target, new Tile(Engine.Elements.SlimeId, color), slimeElement.Cycle, null);
                        Engine.Actors[Engine.State.ActorCount].P2 = actor.P2;
                    }

                    spawnCount++;
                }
            }

            if (spawnCount == 0)
            {
                Engine.RemoveActor(index);
                Engine.Tiles[source] = slimeTrailTile;
                Engine.UpdateBoard(source);
            }
        }
        else
        {
            actor.P1++;
        }
    }
}