using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class ScrollBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly ITiles _tiles;
        private readonly IEngine _engine;
        private readonly IConfig _config;
        private readonly ISounder _sounder;
        private readonly IDrawer _drawer;
        private readonly IMover _mover;

        public override string KnownName => KnownNames.Scroll;

        public ScrollBehavior(IActors actors, ITiles tiles, IEngine engine, IConfig config, ISounder sounder, IDrawer drawer, IMover mover)
        {
            _actors = actors;
            _tiles = tiles;
            _engine = engine;
            _config = config;
            _sounder = sounder;
            _drawer = drawer;
            _mover = mover;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];
            var color = _tiles[actor.Location].Color;

            color++;
            if (color > 0x0F)
            {
                color = 0x09;
            }
            _tiles[actor.Location].Color = color;
            _drawer.UpdateBoard(actor.Location);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            var scrollIndex = _actors.ActorIndexAt(location);
            var actor = _actors[scrollIndex];

            _sounder.Play(2, _sounder.Encode(_config.ScrollMusic));
            _engine.ExecuteCode(scrollIndex, actor, _config.ScrollTitle);
            _mover.RemoveActor(scrollIndex);
        }
    }
}