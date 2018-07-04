using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class PassageBehavior : ElementBehavior
    {
        private readonly IConfig _config;
        private readonly IEngine _engine;
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly IGrid _grid;
        private readonly ISounds _sounds;
        private readonly IState _state;

        public PassageBehavior(IConfig config, IEngine engine, IActors actors, IElements elements, IGrid grid, ISounds sounds, IState state)
        {
            _config = config;
            _engine = engine;
            _actors = actors;
            _elements = elements;
            _grid = grid;
            _sounds = sounds;
            _state = state;
        }

        public override string KnownName => KnownNames.Passage;

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var searchColor = _grid[location].Color;
            var passageIndex = _actors.ActorIndexAt(location);
            var passageTarget = _actors[passageIndex].P3;
            _engine.SetBoard(passageTarget);
            var target = new Location();

            for (var x = 1; x <= _grid.Width; x++)
            {
                for (var y = 1; y <= _grid.Height; y++)
                {
                    if (_grid.TileAt(x, y).Id == _elements.PassageId)
                    {
                        if (_grid.TileAt(x, y).Color == searchColor)
                        {
                            target.SetTo(x, y);
                        }
                    }
                }
            }

            if (_config.BuggyPassages)
            {
                // this is what causes the black holes when using passages
                _grid[_actors.Player.Location].SetTo(_elements.EmptyId, 0);
            }
            else
            {
                // Passage holes were fixed in Super ZZT
                _grid[_actors.Player.Location].CopyFrom(_actors.Player.UnderTile);
            }

            if (target.X != 0)
            {
                _actors.Player.Location.CopyFrom(target);
            }
            _state.GamePaused = true;
            _engine.PlaySound(4, _sounds.Passage);
            _engine.FadePurple();
            _engine.EnterBoard();
            vector.SetTo(0, 0);
        }
    }
}