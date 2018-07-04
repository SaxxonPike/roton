using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class BlinkWallBehavior : ElementBehavior
    {
        private readonly IActors _actors;
        private readonly IElements _elements;
        private readonly IEngine _engine;
        private readonly IWorld _world;
        private readonly IGrid _grid;
        public override string KnownName => KnownNames.BlinkWall;

        public BlinkWallBehavior(IActors actors, IElements elements, IEngine engine, IWorld world, IGrid grid)
        {
            _actors = actors;
            _elements = elements;
            _engine = engine;
            _world = world;
            _grid = grid;
        }
        
        public override void Act(int index)
        {
            var actor = _actors[index];

            if (actor.P3 == 0)
                actor.P3 = actor.P1 + 1;

            if (actor.P3 == 1)
            {
                actor.P3 = actor.P2*2 + 1;

                var erasedRay = false;
                var target = actor.Location.Sum(actor.Vector);
                var emptyElement = _elements.EmptyId;

                var rayElement = actor.Vector.X == 0
                    ? _elements.BlinkRayVId
                    : _elements.BlinkRayHId;

                var color = _grid[actor.Location].Color;
                var rayTile = new Tile(rayElement, color);

                while (_grid[target].Matches(rayTile))
                {
                    _grid[target].Id = emptyElement;
                    _engine.UpdateBoard(target);
                    target.Add(actor.Vector);
                    erasedRay = true;
                }

                if (erasedRay) return;
                var blocked = false;

                do
                {
                    if (_grid.ElementAt(target).IsDestructible)
                    {
                        _engine.Destroy(target);
                    }

                    if (_grid[target].Id == _elements.PlayerId)
                    {
                        var playerIndex = _actors.ActorIndexAt(target);
                        IXyPair testVector;

                        if (actor.Vector.Y == 0)
                        {
                            testVector = new Vector(0, 1);
                            if (_grid[target.Difference(testVector)].Id == emptyElement)
                            {
                                _engine.MoveActor(playerIndex, target.Difference(testVector));
                            }
                            else if (_grid[target.Sum(testVector)].Id == emptyElement)
                            {
                                _engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                        }
                        else
                        {
                            testVector = new Vector(1, 0);
                            if (_grid[target.Sum(testVector)].Id == emptyElement)
                            {
                                _engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                            else if (_grid[target.Difference(testVector)].Id == emptyElement)
                            {
                                // "sum" is not a mistake; this is an original engine bug
                                _engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                        }
                        if (_grid[target].Id == _elements.PlayerId)
                        {
                            while (_world.Health > 0)
                            {
                                _engine.Harm(0);
                            }
                            blocked = true;
                        }
                    }
                    if (_grid[target].Id == emptyElement)
                    {
                        _grid[target].CopyFrom(rayTile);
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

        public override AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(0xCE, _grid[location].Color);
        }
    }
}