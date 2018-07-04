using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class BlinkWallBehavior : ElementBehavior
    {
        private readonly IActorList _actorList;
        private readonly IElementList _elementList;
        private readonly IEngine _engine;
        private readonly IWorld _world;
        private readonly ITileGrid _tileGrid;
        public override string KnownName => KnownNames.BlinkWall;

        public BlinkWallBehavior(IActorList actorList, IElementList elementList, IEngine engine, IWorld world, ITileGrid tileGrid)
        {
            _actorList = actorList;
            _elementList = elementList;
            _engine = engine;
            _world = world;
            _tileGrid = tileGrid;
        }
        
        public override void Act(int index)
        {
            var actor = _actorList[index];

            if (actor.P3 == 0)
                actor.P3 = actor.P1 + 1;

            if (actor.P3 == 1)
            {
                actor.P3 = actor.P2*2 + 1;

                var erasedRay = false;
                var target = actor.Location.Sum(actor.Vector);
                var emptyElement = _elementList.EmptyId;

                var rayElement = actor.Vector.X == 0
                    ? _elementList.BlinkRayVId
                    : _elementList.BlinkRayHId;

                var color = _engine.TileAt(actor.Location).Color;
                var rayTile = new Tile(rayElement, color);

                while (_engine.TileAt(target).Matches(rayTile))
                {
                    _engine.TileAt(target).Id = emptyElement;
                    _engine.UpdateBoard(target);
                    target.Add(actor.Vector);
                    erasedRay = true;
                }

                if (erasedRay) return;
                var blocked = false;

                do
                {
                    if (_engine.ElementAt(target).IsDestructible)
                    {
                        _engine.Destroy(target);
                    }

                    if (_engine.TileAt(target).Id == _elementList.PlayerId)
                    {
                        var playerIndex = _engine.ActorIndexAt(target);
                        IXyPair testVector;

                        if (actor.Vector.Y == 0)
                        {
                            testVector = new Vector(0, 1);
                            if (_engine.TileAt(target.Difference(testVector)).Id == emptyElement)
                            {
                                _engine.MoveActor(playerIndex, target.Difference(testVector));
                            }
                            else if (_engine.TileAt(target.Sum(testVector)).Id == emptyElement)
                            {
                                _engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                        }
                        else
                        {
                            testVector = new Vector(1, 0);
                            if (_engine.TileAt(target.Sum(testVector)).Id == emptyElement)
                            {
                                _engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                            else if (_engine.TileAt(target.Difference(testVector)).Id == emptyElement)
                            {
                                // "sum" is not a mistake; this is an original engine bug
                                _engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                        }
                        if (_engine.TileAt(target).Id == _elementList.PlayerId)
                        {
                            while (_world.Health > 0)
                            {
                                _engine.Harm(0);
                            }
                            blocked = true;
                        }
                    }
                    if (_engine.TileAt(target).Id == emptyElement)
                    {
                        _engine.TileAt(target).CopyFrom(rayTile);
                        _engine.UpdateBoard(target);
                    }
                    else
                    {
                        blocked = true;
                    }
                    target.Add(actor.Vector);
                } while (!blocked);
            }
            else
            {
                actor.P3--;
            }
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            return new AnsiChar(0xCE, _tileGrid[location].Color);
        }
    }
}