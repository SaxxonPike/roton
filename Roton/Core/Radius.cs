using Roton.Emulation.Behavior;
using Roton.Emulation.Execution;
using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Core
{
    public class Radius : IRadius
    {
        private readonly IActors _actors;
        private readonly IBroadcaster _broadcaster;
        private readonly IElements _elements;
        private readonly IRandom _random;
        private readonly IDrawer _drawer;
        private readonly IMover _mover;
        private readonly ITiles _tiles;

        public Radius(IActors actors, IBroadcaster broadcaster, IElements elements,
            IRandom random, IDrawer drawer, IMover mover, ITiles tiles)
        {
            _actors = actors;
            _broadcaster = broadcaster;
            _elements = elements;
            _random = random;
            _drawer = drawer;
            _mover = mover;
            _tiles = tiles;
        }

        public void Update(IXyPair location, RadiusMode mode)
        {
            var source = location.Clone();
            var left = source.X - 9;
            var right = source.X + 9;
            var top = source.Y - 6;
            var bottom = source.Y + 6;
            for (var x = left; x <= right; x++)
            {
                for (var y = top; y <= bottom; y++)
                {
                    if (x >= 1 && x <= _tiles.Width && y >= 1 && y <= _tiles.Height)
                    {
                        var target = new Location(x, y);
                        if (mode != RadiusMode.Update)
                        {
                            if (source.DistanceTo(target) < 50)
                            {
                                var element = _tiles.ElementAt(target);
                                if (mode == RadiusMode.Explode)
                                {
                                    if (element.CodeEditText.Length > 0)
                                    {
                                        var actorIndex = _actors.ActorIndexAt(target);
                                        if (actorIndex > 0)
                                        {
                                            _broadcaster.BroadcastLabel(-actorIndex, KnownLabels.Bombed, false);
                                        }
                                    }

                                    if (element.IsDestructible || element.Id == _elements.StarId)
                                    {
                                        _mover.Destroy(target);
                                    }

                                    if (element.Id == _elements.EmptyId || element.Id == _elements.BreakableId)
                                    {
                                        _tiles[target].SetTo(_elements.BreakableId, _random.Synced(7) + 9);
                                    }
                                }
                                else
                                {
                                    if (_tiles[target].Id == _elements.BreakableId)
                                    {
                                        _tiles[target].Id = _elements.EmptyId;
                                    }
                                }
                            }
                        }

                        _drawer.UpdateBoard(target);
                    }
                }
            }
        }
    }
}