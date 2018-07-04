using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class ScrollBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IGrid _grid;
        private readonly IEngine _engine;
        private readonly IConfig _config;

        public override string KnownName => KnownNames.Scroll;

        public ScrollBehavior(IActors actors, IGrid grid, IEngine engine, IConfig config)
        {
            _actors = actors;
            _grid = grid;
            _engine = engine;
            _config = config;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            var color = _grid[actor.Location].Color;

            color++;
            if (color > 0x0F)
            {
                color = 0x09;
            }
            _grid[actor.Location].Color = color;
            _engine.UpdateBoard(actor.Location);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var scrollIndex = _actors.ActorIndexAt(location);
            var actor = _actors[scrollIndex];

            _engine.PlaySound(2, _engine.EncodeMusic(_config.ScrollMusic));
            _engine.ExecuteCode(scrollIndex, actor, _config.ScrollTitle);
            _engine.RemoveActor(scrollIndex);
        }
    }
}