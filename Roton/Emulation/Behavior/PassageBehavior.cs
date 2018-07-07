using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class PassageBehavior : ElementBehavior
    {
        private readonly IConfig _config;
        private readonly IEngine _engine;
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly ITiles _tiles;
        private readonly ISounds _sounds;
        private readonly IState _state;
        private readonly ISounder _sounder;

        public PassageBehavior(IConfig config, IEngine engine, IActors actors, IElements elements, ITiles tiles,
            ISounds sounds, IState state, ISounder sounder)
        {
            _config = config;
            _engine = engine;
            _actors = actors;
            _elements = elements;
            _tiles = tiles;
            _sounds = sounds;
            _state = state;
            _sounder = sounder;
        }

        public override string KnownName => KnownNames.Passage;

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var searchColor = _tiles[location].Color;
            var passageIndex = _actors.ActorIndexAt(location);
            var passageTarget = _actors[passageIndex].P3;
            _engine.SetBoard(passageTarget);
            var target = new Location();

            for (var x = 1; x <= _tiles.Width; x++)
            {
                for (var y = 1; y <= _tiles.Height; y++)
                {
                    var loc = new Location(x, y);
                    if (_tiles[loc].Id == _elements.PassageId)
                    {
                        if (_tiles[loc].Color == searchColor)
                        {
                            target.SetTo(x, y);
                        }
                    }
                }
            }

            if (_config.BuggyPassages)
            {
                // this is what causes the black holes when using passages
                _tiles[_actors.Player.Location].SetTo(_elements.EmptyId, 0);
            }
            else
            {
                // Passage holes were fixed in Super ZZT
                _tiles[_actors.Player.Location].CopyFrom(_actors.Player.UnderTile);
            }

            if (target.X != 0)
            {
                _actors.Player.Location.CopyFrom(target);
            }

            _state.GamePaused = true;
            _sounder.Play(4, _sounds.Passage);
            _engine.FadePurple();
            _engine.EnterBoard();
            vector.SetTo(0, 0);
        }
    }
}