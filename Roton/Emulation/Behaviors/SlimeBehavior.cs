using Roton.Core;
using Roton.Emulation.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class SlimeBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Slime;

        public SlimeBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            var actor = _engine.Actors[index];

            if (actor.P1 >= actor.P2)
            {
                var spawnCount = 0;
                var color = _engine.Tiles[actor.Location].Color;
                var slimeElement = _engine.Elements[_engine.Elements.SlimeId];
                var slimeTrailTile = new Tile(_engine.Elements.BreakableId, color);
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
                            _engine.SpawnActor(target, new Tile(_engine.Elements.SlimeId, color), slimeElement.Cycle, null);
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

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var color = _engine.Tiles[location].Color;
            var slimeIndex = _engine.ActorIndexAt(location);
            _engine.Harm(slimeIndex);
            _engine.Tiles[location].SetTo(_engine.Elements.BreakableId, color);
            _engine.UpdateBoard(location);
            _engine.PlaySound(2, _engine.Sounds.SlimeDie);
        }
    }
}