using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class MessengerBehavior : ElementBehavior
    {
        public override string KnownName => "Messenger";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            if (actor.Location.X == 0)
            {
                engine.Hud.DrawMessage(new Message(engine.State.Message, engine.State.Message2), actor.P2%7 + 9);
                actor.P2--;
                if (actor.P2 > 0) return;

                engine.RemoveActor(index);
                engine.State.ActIndex--;
                engine.Hud.UpdateBorder();
                engine.State.Message = string.Empty;
            }
        }
    }
}