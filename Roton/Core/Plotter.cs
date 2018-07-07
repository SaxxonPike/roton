using System;
using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Core
{
    public class Plotter : IPlotter
    {
        private readonly IElements _elements;
        private readonly IActors _actors;
        private readonly IState _state;
        private readonly Lazy<IDrawer> _drawer;
        private readonly Lazy<ISpawner> _spawner;
        private readonly Lazy<IMover> _mover;
        private readonly ITiles _tiles;

        public Plotter(IElements elements, IActors actors,
            IState state,
            Lazy<IDrawer> drawer, Lazy<ISpawner> spawner,
            Lazy<IMover> mover, ITiles tiles)
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
                _drawer.Value.UpdateBoard(actor.Location);
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
                _mover.Value.Destroy(location);
                if (targetElement.Cycle < 0)
                {
                    existingTile.SetTo(targetElement.Id, targetColor);
                }
                else
                {
                    _spawner.Value.SpawnActor(location, new Tile(targetElement.Id, targetColor), targetElement.Cycle,
                        _state.DefaultActor);
                }
            }

            _drawer.Value.UpdateBoard(location);
        }

        public void Put(IXyPair location, IXyPair vector, ITile kind)
        {
            if (location.X >= 1 && location.X <= _tiles.Width && location.Y >= 1 &&
                location.Y <= _tiles.Height)
            {
                if (!_tiles.ElementAt(location).IsFloor)
                {
                    _mover.Value.Push(location, vector);
                }

                PlotTile(location, kind);
            }
        }

        public virtual void RemoveItem(IXyPair location)
        {
            _tiles[location].Id = _elements.EmptyId;
            _drawer.Value.UpdateBoard(location);
        }
    }
}