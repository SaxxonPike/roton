using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class SlimeBehavior : ElementBehavior
    {
        public override string KnownName => "Slime";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];

            if (actor.P1 >= actor.P2)
            {
                var spawnCount = 0;
                var color = engine.Tiles[actor.Location].Color;
                var slimeElement = engine.Elements.SlimeElement;
                var slimeTrailTile = new Tile(engine.Elements.BreakableId, color);
                var source = actor.Location.Clone();
                actor.P1 = 0;

                for (var i = 0; i < 4; i++)
                {
                    var target = source.Sum(engine.GetCardinalVector(i));
                    if (engine.ElementAt(target).IsFloor)
                    {
                        if (spawnCount == 0)
                        {
                            engine.MoveActor(index, target);
                            engine.Tiles[source].CopyFrom(slimeTrailTile);
                            engine.UpdateBoard(source);
                        }
                        else
                        {
                            engine.SpawnActor(target, new Tile(engine.Elements.SlimeId, color), slimeElement.Cycle, null);
                            engine.Actors[engine.StateData.ActorCount].P2 = actor.P2;
                        }
                        spawnCount++;
                    }
                }

                if (spawnCount == 0)
                {
                    engine.RemoveActor(index);
                    engine.Tiles[source].CopyFrom(slimeTrailTile);
                    engine.UpdateBoard(source);
                }
            }
            else
            {
                actor.P1++;
            }
        }

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            var color = engine.Tiles[location].Color;
            var slimeIndex = engine.ActorIndexAt(location);
            engine.Harm(slimeIndex);
            engine.Tiles[location].SetTo(engine.Elements.BreakableId, color);
            engine.UpdateBoard(location);
            engine.PlaySound(2, engine.SoundSet.SlimeDie);
        }
    }
}
