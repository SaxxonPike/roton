using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class ScrollBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        public override string KnownName => KnownNames.Scroll;

        public ScrollBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            var actor = _engine.Actors[index];
            var color = _engine.Tiles[actor.Location].Color;

            color++;
            if (color > 0x0F)
            {
                color = 0x09;
            }
            _engine.Tiles[actor.Location].Color = color;
            _engine.UpdateBoard(actor.Location);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var scrollIndex = _engine.ActorIndexAt(location);
            var actor = _engine.Actors[scrollIndex];

            _engine.PlaySound(2, _engine.EncodeMusic(_engine.Config.ScrollMusic));
            _engine.ExecuteCode(scrollIndex, actor, _engine.Config.ScrollTitle);
            _engine.RemoveActor(scrollIndex);
        }
    }
}