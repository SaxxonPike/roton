using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original, 0x0D)]
    [ContextEngine(ContextEngine.Super, 0x0D)]
    public sealed class BombAction : IAction
    {
        private readonly IEngine _engine;

        public BombAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = _engine.Actors[index];
            if (actor.P1 <= 0) return;

            actor.P1--;
            _engine.UpdateBoard(actor.Location);
            switch (actor.P1)
            {
                case 1:
                    _engine.PlaySound(1, _engine.Sounds.BombExplode);
                    _engine.UpdateRadius(actor.Location, RadiusMode.Explode);
                    break;
                case 0:
                    var location = actor.Location.Clone();
                    _engine.RemoveActor(index);
                    _engine.UpdateRadius(location, RadiusMode.Clear);
                    break;
                default:
                    _engine.PlaySound(1, (actor.P1 & 0x01) == 0 ? _engine.Sounds.BombTock : _engine.Sounds.BombTick);
                    break;
            }
        }
    }
}