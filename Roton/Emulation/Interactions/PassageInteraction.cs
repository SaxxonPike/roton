using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Interactions
{
    [ContextEngine(ContextEngine.Original, 0x0B)]
    [ContextEngine(ContextEngine.Super, 0x0B)]
    public sealed class PassageInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public PassageInteraction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Interact(IXyPair location, int index, IXyPair vector)
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
                    if (_engine.Tiles[loc].Id == _engine.ElementList.PassageId && _engine.Tiles[loc].Color == searchColor)
                        target.SetTo(x, y);
                }
            }

            _engine.CleanUpPassageMovement();

            if (target.X != 0)
                _engine.Player.Location.CopyFrom(target);

            _engine.State.GamePaused = true;
            _engine.PlaySound(4, _engine.Sounds.Passage);
            _engine.FadePurple();
            _engine.EnterBoard();
            vector.SetTo(0, 0);
        }
    }
}