using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl
{
    [ContextEngine(ContextEngine.Original, 0x1D)]
    [ContextEngine(ContextEngine.Super, 0x1D)]
    public sealed class BlinkWallAction : IAction
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public BlinkWallAction(Lazy<IEngine> engine)
        {
            _engine = engine;
        }
        
        public void Act(int index)
        {
            var actor = Engine.Actors[index];

            if (actor.P3 == 0)
                actor.P3 = actor.P1 + 1;

            if (actor.P3 == 1)
            {
                actor.P3 = actor.P2 * 2 + 1;

                var erasedRay = false;
                var target = actor.Location.Sum(actor.Vector);
                var emptyElement = Engine.ElementList.EmptyId;

                var rayElement = actor.Vector.X == 0
                    ? Engine.ElementList.BlinkRayVId
                    : Engine.ElementList.BlinkRayHId;

                var color = Engine.Tiles[actor.Location].Color;
                var rayTile = new Tile(rayElement, color);

                while (Engine.Tiles[target].Matches(rayTile))
                {
                    Engine.Tiles[target].Id = emptyElement;
                    Engine.UpdateBoard(target);
                    target.Add(actor.Vector);
                    erasedRay = true;
                }

                if (erasedRay) return;
                var blocked = false;

                do
                {
                    if (Engine.Tiles.ElementAt(target).IsDestructible)
                    {
                        Engine.Destroy(target);
                    }

                    if (Engine.Tiles[target].Id == Engine.ElementList.PlayerId)
                    {
                        var playerIndex = Engine.Actors.ActorIndexAt(target);
                        IXyPair testVector;

                        if (actor.Vector.Y == 0)
                        {
                            testVector = new Vector(0, 1);
                            if (Engine.Tiles[target.Difference(testVector)].Id == emptyElement)
                            {
                                Engine.MoveActor(playerIndex, target.Difference(testVector));
                            }
                            else if (Engine.Tiles[target.Sum(testVector)].Id == emptyElement)
                            {
                                Engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                        }
                        else
                        {
                            testVector = new Vector(1, 0);
                            if (Engine.Tiles[target.Sum(testVector)].Id == emptyElement)
                            {
                                Engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                            else if (Engine.Tiles[target.Difference(testVector)].Id == emptyElement)
                            {
                                // "sum" is not a mistake; this is an original engine bug
                                Engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                        }

                        if (Engine.Tiles[target].Id == Engine.ElementList.PlayerId)
                        {
                            while (Engine.World.Health > 0)
                            {
                                Engine.Harm(0);
                            }

                            blocked = true;
                        }
                    }

                    if (Engine.Tiles[target].Id == emptyElement)
                    {
                        Engine.Tiles[target].CopyFrom(rayTile);
                        Engine.UpdateBoard(target);
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