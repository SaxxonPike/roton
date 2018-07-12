using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Super, 0x40)]
    public sealed class StoneAction : IAction
    {
        private readonly IEngine _engine;

        public StoneAction(IEngine engine)
        {
            _engine = engine;
        }

        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            _engine.Tiles[actor.Location].Color =
                (_engine.Tiles[actor.Location].Color & 0x70) + _engine.Random.Synced(7) + 9;
            _engine.UpdateBoard(_engine.Actors[index].Location);
        }
    }
}