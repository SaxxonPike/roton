using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class BlinkWallBehavior : ElementBehavior
    {
        public override string KnownName => "Blink Wall";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];

            if (actor.P3 == 0)
                actor.P3 = actor.P1 + 1;

            if (actor.P3 == 1)
            {
                actor.P3 = actor.P2*2 + 1;

                var erasedRay = false;
                var target = actor.Location.Sum(actor.Vector);
                var emptyElement = engine.Elements.EmptyId;

                var rayElement = actor.Vector.X == 0
                    ? engine.Elements.BlinkRayVId
                    : engine.Elements.BlinkRayHId;

                var color = engine.TileAt(actor.Location).Color;
                var rayTile = new Tile(rayElement, color);

                while (engine.TileAt(target).Matches(rayTile))
                {
                    engine.TileAt(target).Id = emptyElement;
                    engine.UpdateBoard(target);
                    target.Add(actor.Vector);
                    erasedRay = true;
                }

                if (erasedRay) return;
                var blocked = false;

                do
                {
                    if (engine.ElementAt(target).IsDestructible)
                    {
                        engine.Destroy(target);
                    }

                    if (engine.TileAt(target).Id == engine.Elements.PlayerId)
                    {
                        var playerIndex = engine.ActorIndexAt(target);
                        IXyPair testVector;

                        if (actor.Vector.Y == 0)
                        {
                            testVector = new Vector(0, 1);
                            if (engine.TileAt(target.Difference(testVector)).Id == emptyElement)
                            {
                                engine.MoveActor(playerIndex, target.Difference(testVector));
                            }
                            else if (engine.TileAt(target.Sum(testVector)).Id == emptyElement)
                            {
                                engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                        }
                        else
                        {
                            testVector = new Vector(1, 0);
                            if (engine.TileAt(target.Sum(testVector)).Id == emptyElement)
                            {
                                engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                            else if (engine.TileAt(target.Difference(testVector)).Id == emptyElement)
                            {
                                // "sum" is not a mistake; this is an original engine bug
                                engine.MoveActor(playerIndex, target.Sum(testVector));
                            }
                        }
                        if (engine.TileAt(target).Id == engine.Elements.PlayerId)
                        {
                            while (engine.World.Health > 0)
                            {
                                engine.Harm(0);
                            }
                            blocked = true;
                        }
                    }
                    if (engine.TileAt(target).Id == emptyElement)
                    {
                        engine.TileAt(target).CopyFrom(rayTile);
                        engine.UpdateBoard(target);
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
            return new AnsiChar(0xCE, engine.Tiles[location].Color);
        }
    }
}