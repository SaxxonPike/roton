﻿using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class MessengerBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IHud _hud;
        private readonly IState _state;
        private readonly IEngine _engine;
        
        public override string KnownName => KnownNames.Messenger;

        public MessengerBehavior(IActors actors, IHud hud, IState state, IEngine engine)
        {
            _actors = actors;
            _hud = hud;
            _state = state;
            _engine = engine;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            if (actor.Location.X == 0)
            {
                _hud.DrawMessage(new Message(_state.Message, _state.Message2), actor.P2 % 7 + 9);
                actor.P2--;
                if (actor.P2 > 0) return;

                _engine.RemoveActor(index);
                _state.ActIndex--;
                _hud.UpdateBorder();
                _state.Message = string.Empty;
            }
        }
    }
}