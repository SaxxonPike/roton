using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Interactions
{
    public class BombInteraction : IInteraction
    {
        private readonly IEngine _engine;

        public BombInteraction(IEngine engine)
        {
            _engine = engine;
        }
        public void Interact(IXyPair location, int index, IXyPair vector)
        {
            var actor = _engine.ActorAt(location);
            if (actor.P1 == 0)
            {
                actor.P1 = 9;
                _engine.UpdateBoard(location);
                _engine.SetMessage(0xC8, _engine.Alerts.BombMessage);
                _engine.PlaySound(4, _engine.Sounds.BombActivate);
            }
            else
            {
                _engine.Push(location, vector);
            }
        }
    }
}