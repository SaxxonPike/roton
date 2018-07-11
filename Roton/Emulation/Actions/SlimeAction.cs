using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Zzt, 0x25)]
    [ContextEngine(ContextEngine.SuperZzt, 0x25)]
    public sealed class SlimeAction : IAction
    {
        private readonly IEngine _engine;
        
        public SlimeAction(IEngine engine)
        {
            _engine = engine;
        }

        public void Act(int index)
        {
            var actor = _engine.Actors[index];

            if (actor.P1 >= actor.P2)
            {
                var spawnCount = 0;
                var color = _engine.Tiles[actor.Location].Color;
                var slimeElement = _engine.ElementList[_engine.ElementList.SlimeId];
                var slimeTrailTile = new Tile(_engine.ElementList.BreakableId, color);
                var source = actor.Location.Clone();
                actor.P1 = 0;

                for (var i = 0; i < 4; i++)
                {
                    var target = source.Sum(_engine.GetCardinalVector(i));
                    if (_engine.Tiles.ElementAt(target).IsFloor)
                    {
                        if (spawnCount == 0)
                        {
                            _engine.MoveActor(index, target);
                            _engine.Tiles[source].CopyFrom(slimeTrailTile);
                            _engine.UpdateBoard(source);
                        }
                        else
                        {
                            _engine.SpawnActor(target, new Tile(_engine.ElementList.SlimeId, color), slimeElement.Cycle, null);
                            _engine.Actors[_engine.State.ActorCount].P2 = actor.P2;
                        }

                        spawnCount++;
                    }
                }

                if (spawnCount == 0)
                {
                    _engine.RemoveActor(index);
                    _engine.Tiles[source].CopyFrom(slimeTrailTile);
                    _engine.UpdateBoard(source);
                }
            }
            else
            {
                actor.P1++;
            }
        }
    }
}