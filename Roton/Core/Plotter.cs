using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Core
{
    public class Plotter : IPlotter
    {
        private readonly IElements _elements;
        private readonly IActors _actors;
        private readonly IState _state;
        private readonly IDrawer _drawer;
        private readonly ISpawner _spawner;
        private readonly IMover _mover;
        private readonly ITiles _tiles;

        public Plotter(IElements elements, IActors actors,
            IState state,
            IDrawer drawer, ISpawner spawner,
            IMover mover, ITiles tiles)
        {
            _elements = elements;
            _actors = actors;
            _state = state;
            _drawer = drawer;
            _spawner = spawner;
            _mover = mover;
            _tiles = tiles;
        }

        public virtual void ForcePlayerColor(int index)
        {
            var actor = _actors[index];
            var playerElement = _elements[_elements.PlayerId];
            if (_tiles[actor.Location].Color != playerElement.Color ||
                playerElement.Character != 0x02)
            {
                playerElement.Character = 2;
                _tiles[actor.Location].Color = playerElement.Color;
                _drawer.UpdateBoard(actor.Location);
            }
        }

        public void PlotTile(IXyPair location, ITile tile)
        {
            if (_tiles.ElementAt(location).Id == _elements.PlayerId)
                return;

            var targetElement = _elements[tile.Id];
            var existingTile = _tiles[location];
            var targetColor = tile.Color;
            if (targetElement.Color >= 0xF0)
            {
                if (targetColor == 0)
                    targetColor = existingTile.Color;
                if (targetColor == 0)
                    targetColor = 0x0F;
                if (targetElement.Color == 0xFE)
                    targetColor = ((targetColor - 8) << 4) + 0x0F;
            }
            else
            {
                targetColor = targetElement.Color;
            }

            if (targetElement.Id == existingTile.Id)
            {
                existingTile.Color = targetColor;
            }
            else
            {
                _mover.Destroy(location);
                if (targetElement.Cycle < 0)
                {
                    existingTile.SetTo(targetElement.Id, targetColor);
                }
                else
                {
                    _spawner.SpawnActor(location, new Tile(targetElement.Id, targetColor), targetElement.Cycle,
                        _state.DefaultActor);
                }
            }

            _drawer.UpdateBoard(location);
        }

        public void Put(IXyPair location, IXyPair vector, ITile kind)
        {
            if (location.X >= 1 && location.X <= _tiles.Width && location.Y >= 1 &&
                location.Y <= _tiles.Height)
            {
                if (!_tiles.ElementAt(location).IsFloor)
                {
                    _mover.Push(location, vector);
                }

                PlotTile(location, kind);
            }
        }

        public virtual void RemoveItem(IXyPair location)
        {
            _tiles[location].Id = _elements.EmptyId;
            _drawer.UpdateBoard(location);
        }
    }
}