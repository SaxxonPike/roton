using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original, 0x25)]
    [ContextEngine(ContextEngine.Super, 0x25)]
    public sealed class SlimeAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public SlimeAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public void Act(int index)
        {
            var actor = Engine.Actors[index];

            if (actor.P1 >= actor.P2)
            {
                var spawnCount = 0;
                var color = Engine.Tiles[actor.Location].Color;
                var slimeElement = Engine.ElementList[Engine.ElementList.SlimeId];
                var slimeTrailTile = new Tile(Engine.ElementList.BreakableId, color);
                var source = actor.Location.Clone();
                actor.P1 = 0;

                for (var i = 0; i < 4; i++)
                {
                    var target = source.Sum(Engine.GetCardinalVector(i));
                    if (Engine.Tiles.ElementAt(target).IsFloor)
                    {
                        if (spawnCount == 0)
                        {
                            Engine.MoveActor(index, target);
                            Engine.Tiles[source].CopyFrom(slimeTrailTile);
                            Engine.UpdateBoard(source);
                        }
                        else
                        {
                            Engine.SpawnActor(target, new Tile(Engine.ElementList.SlimeId, color), slimeElement.Cycle, null);
                            Engine.Actors[Engine.State.ActorCount].P2 = actor.P2;
                        }

                        spawnCount++;
                    }
                }

                if (spawnCount == 0)
                {
                    Engine.RemoveActor(index);
                    Engine.Tiles[source].CopyFrom(slimeTrailTile);
                    Engine.UpdateBoard(source);
                }
            }
            else
            {
                actor.P1++;
            }
        }
    }
}