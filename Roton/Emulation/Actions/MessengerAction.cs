using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Actions
{
    [ContextEngine(ContextEngine.Original, 0x02)]
    [ContextEngine(ContextEngine.Super, 0x02)]
    public sealed class MessengerAction : IAction
    {
        private readonly IEngine _engine;

        public MessengerAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
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