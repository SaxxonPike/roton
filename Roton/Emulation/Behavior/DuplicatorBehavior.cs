using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal class DuplicatorBehavior : ElementBehavior
    {
        public override string KnownName => "Duplicator";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            var source = actor.Location.Sum(actor.Vector);
            var target = actor.Location.Difference(actor.Vector);

            if (actor.P1 > 4)
            {
                if (engine.TileAt(target).Id == engine.Elements.PlayerId)
                {
                    engine.ElementAt(source).Interact(engine, source, 0, engine.State.KeyVector);
                }
                else
                {
                    if (engine.TileAt(target).Id != engine.Elements.EmptyId)
                    {
                        engine.Push(target, actor.Vector.Opposite());
                    }
                    if (engine.TileAt(target).Id == engine.Elements.EmptyId)
                    {
                        var sourceIndex = engine.ActorIndexAt(source);
                        if (sourceIndex > 0)
                        {
                            if (engine.State.ActorCount < engine.Actors.Capacity - 2)
                            {
                                var sourceTile = engine.TileAt(source);
                                engine.SpawnActor(target, sourceTile, engine.Elements[sourceTile.Id].Cycle,
                                    engine.Actors[sourceIndex]);
                                engine.UpdateBoard(target);
                            }
                        }
                        else if (sourceIndex != 0)
                        {
                            engine.TileAt(target).CopyFrom(engine.TileAt(source));
                            engine.UpdateBoard(target);
                        }
                        engine.PlaySound(3, engine.SoundSet.Duplicate);
                    }
                    else
                    {
                        engine.PlaySound(3, engine.SoundSet.DuplicateFail);
                    }
                }
                actor.P1 = 0;
            }
            else
            {
                actor.P1++;
            }

            engine.UpdateBoard(actor.Location);
            actor.Cycle = (9 - actor.P2)*3;
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            switch (engine.ActorAt(location).P1)
            {
                case 2:
                    return new AnsiChar(0xF9, engine.Tiles[location].Color);
                case 3:
                    return new AnsiChar(0xF8, engine.Tiles[location].Color);
                case 4:
                    return new AnsiChar(0x6F, engine.Tiles[location].Color);
                case 5:
                    return new AnsiChar(0x4F, engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0xFA, engine.Tiles[location].Color);
            }
        }
    }
}