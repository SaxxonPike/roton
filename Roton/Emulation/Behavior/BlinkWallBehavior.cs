using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class BlinkWallBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly IWorld _world;
        private readonly ITiles _tiles;
        private readonly IDrawer _drawer;
        private readonly IMover _mover;
        public override string KnownName => KnownNames.BlinkWall;

        public BlinkWallBehavior(IActors actors, IElements elements, IWorld world, ITiles tiles, IDrawer drawer,
            IMover mover)
        {
            _actors = actors;
            _elements = elements;
            _world = world;
            _tiles = tiles;
            _drawer = drawer;
            _mover = mover;
        }

        public override void Act(int index)
        {
            var actor = _actors[index];

            if (actor.P3 == 0)
                actor.P3 = actor.P1 + 1;

            if (actor.P3 == 1)
            {
                actor.P3 = actor.P2 * 2 + 1;

                var erasedRay = false;
                var target = actor.Location.Sum(actor.Vector);
                var emptyElement = _elements.EmptyId;

                var rayElement = actor.Vector.X == 0
                    ? _elements.BlinkRayVId
                    : _elements.BlinkRayHId;

                var color = _tiles[actor.Location].Color;
                var rayTile = new Tile(rayElement, color);

                while (_tiles[target].Matches(rayTile))
                {
                    _tiles[target].Id = emptyElement;
                    _drawer.UpdateBoard(target);
                    target.Add(actor.Vector);
                    erasedRay = true;
                }

                if (erasedRay) return;
                var blocked = false;

                do
                {
                    if (_tiles.ElementAt(target).IsDestructible)
                    {
                        _mover.Destroy(target);
                    }

                    if (_tiles[target].Id == _elements.PlayerId)
                    {
                        var playerIndex = _actors.ActorIndexAt(target);
                        IXyPair testVector;

                        if (actor.Vector.Y == 0)
                        {
                            testVector = new Vector(0, 1);
                            if (_tiles[target.Difference(testVector)].Id == emptyElement)
                            {
                                _mover.MoveActor(playerIndex, target.Difference(testVector));
                            }
                            else if (_tiles[target.Sum(testVector)].Id == emptyElement)
                            {
                                _mover.MoveActor(playerIndex, target.Sum(testVector));
                            }
                        }
                        else
                        {
                            testVector = new Vector(1, 0);
                            if (_tiles[target.Sum(testVector)].Id == emptyElement)
                            {
                                _mover.MoveActor(playerIndex, target.Sum(testVector));
                            }
                            else if (_tiles[target.Difference(testVector)].Id == emptyElement)
                            {
                                // "sum" is not a mistake; this is an original engine bug
                                _mover.MoveActor(playerIndex, target.Sum(testVector));
                            }
                        }

                        if (_tiles[target].Id == _elements.PlayerId)
                        {
                            while (_world.Health > 0)
                            {
                                _mover.Harm(0);
                            }

                            blocked = true;
                        }
                    }

                    if (_tiles[target].Id == emptyElement)
                    {
                        _tiles[target].CopyFrom(rayTile);
                        _drawer.UpdateBoard(target);
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

        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(0xCE, _tiles[location].Color);
        }
    }
}