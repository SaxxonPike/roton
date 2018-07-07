using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Core
{
    public class Drawer : IDrawer
    {
        private readonly IBoard _board;
        private readonly IWorld _world;
        private readonly IActors _actors;
        private readonly IState _state;
        private readonly IElements _elements;
        private readonly IHud _hud;
        private readonly ITiles _tiles;

        public Drawer(IBoard board, IWorld world, IActors actors, IState state, IElements elements,
            IHud hud, ITiles tiles)
        {
            _board = board;
            _world = world;
            _actors = actors;
            _state = state;
            _elements = elements;
            _hud = hud;
            _tiles = tiles;
        }

        public virtual AnsiChar Draw(IXyPair location)
        {
            if (_board.IsDark && !_tiles.ElementAt(location).IsAlwaysVisible &&
                (_world.TorchCycles <= 0 || _actors.Player.Location.DistanceTo(location) >= 50) && !_state.EditorMode)
            {
                return new AnsiChar(0xB0, 0x07);
            }

            var tile = _tiles[location];
            var element = _elements[tile.Id];
            var elementCount = _elements.Count;

            if (tile.Id == _elements.EmptyId)
            {
                return new AnsiChar(0x20, 0x0F);
            }

            if (element.HasDrawCode)
            {
                return element.Draw(location);
            }

            if (tile.Id < elementCount - 7)
            {
                return new AnsiChar(element.Character, tile.Color);
            }

            if (tile.Id != elementCount - 1)
            {
                return new AnsiChar(tile.Color, ((tile.Id - (elementCount - 8)) << 4) | 0x0F);
            }

            return new AnsiChar(tile.Color, 0x0F);
        }

        public void UpdateBoard(IXyPair location)
        {
            _hud.DrawChar(location.X, location.Y, Draw(location));
        }
    }
}