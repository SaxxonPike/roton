using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class SlimeBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly ITiles _tiles;
        private readonly IElements _elements;
        private readonly IState _state;
        private readonly ISounds _sounds;
        private readonly ISounder _sounder;
        private readonly ICompass _compass;
        private readonly IMover _mover;
        private readonly IDrawer _drawer;
        private readonly ISpawner _spawner;

        public override string KnownName => KnownNames.Slime;

        public SlimeBehavior(IActors actors, ITiles tiles, IElements elements, IState state,
            ISounds sounds, ISounder sounder, ICompass compass, IMover mover, IDrawer drawer, ISpawner spawner)
        {
            _actors = actors;
            _tiles = tiles;
            _elements = elements;
            _state = state;
            _sounds = sounds;
            _sounder = sounder;
            _compass = compass;
            _mover = mover;
            _drawer = drawer;
            _spawner = spawner;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];

            if (actor.P1 >= actor.P2)
            {
                var spawnCount = 0;
                var color = _tiles[actor.Location].Color;
                var slimeElement = _elements[_elements.SlimeId];
                var slimeTrailTile = new Tile(_elements.BreakableId, color);
                var source = actor.Location.Clone();
                actor.P1 = 0;

                for (var i = 0; i < 4; i++)
                {
                    var target = source.Sum(_compass.GetCardinalVector(i));
                    if (_tiles.ElementAt(target).IsFloor)
                    {
                        if (spawnCount == 0)
                        {
                            _mover.MoveActor(index, target);
                            _tiles[source].CopyFrom(slimeTrailTile);
                            _drawer.UpdateBoard(source);
                        }
                        else
                        {
                            _spawner.SpawnActor(target, new Tile(_elements.SlimeId, color), slimeElement.Cycle, null);
                            _actors[_state.ActorCount].P2 = actor.P2;
                        }

                        spawnCount++;
                    }
                }

                if (spawnCount == 0)
                {
                    _mover.RemoveActor(index);
                    _tiles[source].CopyFrom(slimeTrailTile);
                    _drawer.UpdateBoard(source);
                }
            }
            else
            {
                actor.P1++;
            }
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var color = _tiles[location].Color;
            var slimeIndex = _actors.ActorIndexAt(location);
            _mover.Harm(slimeIndex);
            _tiles[location].SetTo(_elements.BreakableId, color);
            _drawer.UpdateBoard(location);
            _sounder.Play(2, _sounds.SlimeDie);
        }
    }
}