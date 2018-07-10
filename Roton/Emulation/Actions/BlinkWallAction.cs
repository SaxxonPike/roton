using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Actions
{
    public class BlinkWallAction : IAction
    {
        private readonly IEngine _engine;

        public BlinkWallAction(IEngine engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = _engine.Actors[index];

            if (actor.P3 == 0)
                actor.P3 = actor.P1 + 1;

            if (actor.P3 == 1)
            {
                actor.P3 = actor.P2 * 2 + 1;

                var erasedRay = false;
                var target = actor.Location.Sum(actor.Vector);
                var emptyElement = _engine.ElementList.EmptyId;

                var rayElement = actor.Vector.X == 0
                    ? _engine.ElementList.BlinkRayVId
                    : _engine.ElementList.BlinkRayHId;

                var color = _engine.Tiles[actor.Location].Color;
                var rayTile = new Tile(rayElement, color);

                while (_engine.Tiles[target].Matches(rayTile))
                {
                    _engine.Tiles[target].Id = emptyElement;
                    _engine.UpdateBoard(target);
                    target.Add(actor.Vector);
                    erasedRay = true;
                }

                if (erasedRay) return;
                var blocked = false;

                do
                {
                    if (_engine.Tiles.ElementAt(target).IsDestructible)
                    {
                        _engine.Destroy(target);
                    }

                    if (_engine.Tiles[target].Id == _engine.ElementList.PlayerId)
                    {
                        var playerIndex = _engine.Actors.ActorIndexAt(target);
                        IXyPair testVector;

                        if (actor.Vector.Y == 0)
                        {
                            testVector = new Vector(0, 1);
                            if (_engine.Tiles[target.Difference(testVector)].Id == emptyElement)
                            {
                                _engine.MoveActor(playerIndex, target.Difference(testVector));
                            }
                            else if (_engine.Tiles[target.Sum(testVector)].Id == emptyElement)
                            {
                                _engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                        }
                        else
                        {
                            testVector = new Vector(1, 0);
                            if (_engine.Tiles[target.Sum(testVector)].Id == emptyElement)
                            {
                                _engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                            else if (_engine.Tiles[target.Difference(testVector)].Id == emptyElement)
                            {
                                // "sum" is not a mistake; this is an original engine bug
                                _engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                        }

                        if (_engine.Tiles[target].Id == _engine.ElementList.PlayerId)
                        {
                            while (_engine.World.Health > 0)
                            {
                                _engine.Harm(0);
                            }

                            blocked = true;
                        }
                    }

                    if (_engine.Tiles[target].Id == emptyElement)
                    {
                        _engine.Tiles[target].CopyFrom(rayTile);
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
    }
}