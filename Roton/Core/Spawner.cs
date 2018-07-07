using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Core
{
    public class Spawner : ISpawner
    {
        private readonly IState _state;
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly IWorld _world;
        private readonly ISounder _sounder;
        private readonly ISounds _sounds;
        private readonly IDrawer _drawer;
        private readonly IMover _mover;
        private readonly ITiles _tiles;

        public Spawner(IState state, IActors actors, IElements elements, IWorld world, ISounder sounder,
            ISounds sounds, IDrawer drawer, IMover mover, ITiles tiles)
        {
            _state = state;
            _actors = actors;
            _elements = elements;
            _world = world;
            _sounder = sounder;
            _sounds = sounds;
            _drawer = drawer;
            _mover = mover;
            _tiles = tiles;
        }

        public void SpawnActor(IXyPair location, ITile tile, int cycle, IActor source)
        {
            // must reserve one actor for player, and one for messenger
            if (_state.ActorCount >= _actors.Capacity - 2) 
                return;
            
            _state.ActorCount++;
            var actor = _actors[_state.ActorCount];

            if (source == null)
            {
                source = _state.DefaultActor;
            }

            actor.CopyFrom(source);
            actor.Location.CopyFrom(location);
            actor.Cycle = cycle;
            actor.UnderTile.CopyFrom(_tiles[location]);
            if (_tiles.ElementAt(actor.Location).IsEditorFloor)
            {
                var newColor = _tiles[actor.Location].Color & 0x70;
                newColor |= tile.Color & 0x0F;
                _tiles[actor.Location].Color = newColor;
            }
            else
            {
                _tiles[actor.Location].Color = tile.Color;
            }

            _tiles[actor.Location].Id = tile.Id;
            if (actor.Location.Y > 0)
            {
                _drawer.UpdateBoard(actor.Location);
            }
        }

        public bool SpawnProjectile(int id, IXyPair location, IXyPair vector, bool enemyOwned)
        {
            var target = location.Sum(vector);
            var element = _tiles.ElementAt(target);

            if (element.IsFloor || element.Id == _elements.WaterId)
            {
                SpawnActor(target, new Tile(id, _elements[id].Color), 1, _state.DefaultActor);
                var actor = _actors[_state.ActorCount];
                actor.P1 = enemyOwned ? 1 : 0;
                actor.Vector.CopyFrom(vector);
                actor.P2 = 0x64;
                return true;
            }

            if (element.Id != _elements.BreakableId &&
                (!element.IsDestructible ||
                 (element.Id != _elements.PlayerId || _world.EnergyCycles != 0) && enemyOwned))
            {
                return false;
            }

            _mover.Destroy(target);
            _sounder.Play(2, _sounds.BulletDie);
            return true;
        }
    }
}