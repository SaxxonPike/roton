using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl
{
    [Context(Context.Original, 0x02)]
    [Context(Context.Super, 0x02)]
    public sealed class MessengerAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public MessengerAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = Engine.Actors[index];
            if (actor.Location.X == 0)
            {
                Engine.Hud.DrawMessage(new Message(Engine.GetMessageLines()), actor.P2 % 7 + 9);
                actor.P2--;
                if (actor.P2 > 0) return;

                Engine.RemoveActor(index);
                Engine.State.ActIndex--;
                Engine.Hud.UpdateBorder();
                Engine.State.Message = string.Empty;
                Engine.State.Message2 = string.Empty;
            }
        }
    }
}