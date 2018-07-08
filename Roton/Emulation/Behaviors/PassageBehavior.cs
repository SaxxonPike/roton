using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Behaviors
{
    public sealed class PassageBehavior : ElementBehavior
    {
        private readonly IEngine _engine;

        public PassageBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override string KnownName => KnownNames.Passage;

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var searchColor = _engine.Tiles[location].Color;
            var passageIndex = _engine.ActorIndexAt(location);
            var passageTarget = _engine.Actors[passageIndex].P3;
            _engine.SetBoard(passageTarget);
            var target = new Location();

            for (var x = 1; x <= _engine.Tiles.Width; x++)
            {
                for (var y = 1; y <= _engine.Tiles.Height; y++)
                {
                    var loc = new Location(x, y);
                    if (_engine.Tiles[loc].Id == _engine.Elements.PassageId)
                    {
                        if (_engine.Tiles[loc].Color == searchColor)
                        {
                            target.SetTo(x, y);
                        }
                    }
                }
            }

            if (_engine.Config.BuggyPassages)
            {
                // this is what causes the black holes when using passages
                _engine.Tiles[_engine.Player.Location].SetTo(_engine.Elements.EmptyId, 0);
            }
            else
            {
                // Passage holes were fixed in Super ZZT
                _engine.Tiles[_engine.Player.Location].CopyFrom(_engine.Player.UnderTile);
            }

            if (target.X != 0)
            {
                _engine.Player.Location.CopyFrom(target);
            }

            _engine.State.GamePaused = true;
            _engine.PlaySound(4, _engine.Sounds.Passage);
            _engine.FadePurple();
            _engine.EnterBoard();
            vector.SetTo(0, 0);
        }
    }
}