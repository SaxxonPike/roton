using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Behaviors
{
    public sealed class MessengerBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Messenger;

        public MessengerBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            var actor = _engine.Actors[index];
            if (actor.Location.X == 0)
            {
                _engine.Hud.DrawMessage(new Message(_engine.State.Message, _engine.State.Message2), actor.P2 % 7 + 9);
                actor.P2--;
                if (actor.P2 > 0) return;

                _engine.RemoveActor(index);
                _engine.State.ActIndex--;
                _engine.Hud.UpdateBorder();
                _engine.State.Message = string.Empty;
                _engine.State.Message2 = string.Empty;
            }
        }
    }
}