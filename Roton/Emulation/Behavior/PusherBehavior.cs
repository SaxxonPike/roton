using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    internal sealed class PusherBehavior : ElementBehavior
    {
        public override string KnownName => "Pusher";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            var source = actor.Location.Clone();

            if (!engine.ElementAt(actor.Location.Sum(actor.Vector)).IsFloor)
            {
                engine.Push(actor.Location.Sum(actor.Vector), actor.Vector);
            }

            index = engine.ActorIndexAt(source);
            actor = engine.Actors[index];
            if (!engine.ElementAt(actor.Location.Sum(actor.Vector)).IsFloor) return;

            engine.MoveActor(index, actor.Location.Sum(actor.Vector));
            engine.PlaySound(2, engine.SoundSet.Push);
            var behindLocation = actor.Location.Difference(actor.Vector);
            if (engine.TileAt(behindLocation).Id != engine.Elements.PusherId) return;

            var behindIndex = engine.ActorIndexAt(behindLocation);
            var behindActor = engine.Actors[behindIndex];
            if (behindActor.Vector.X == actor.Vector.X && behindActor.Vector.Y == actor.Vector.Y)
            {
                engine.Elements[engine.Elements.PusherId].Act(engine, behindIndex);
            }
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            var actor = engine.ActorAt(location);
            switch (actor.Vector.X)
            {
                case 1:
                    return new AnsiChar(0x10, engine.Tiles[location].Color);
                case -1:
                    return new AnsiChar(0x11, engine.Tiles[location].Color);
                default:
                    return actor.Vector.Y == -1
                        ? new AnsiChar(0x1E, engine.Tiles[location].Color)
                        : new AnsiChar(0x1F, engine.Tiles[location].Color);
            }
        }
    }
}