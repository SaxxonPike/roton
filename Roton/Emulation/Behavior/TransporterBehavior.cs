using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behavior
{
    public sealed class TransporterBehavior : ElementBehavior
    {
        public override string KnownName => "Transporter";

        public override void Act(int index)
        {
            engine.UpdateBoard(_actorList[index].Location);
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            var actor = engine.ActorAt(location);
            int index;

            if (actor.Vector.X == 0)
            {
                if (actor.Cycle > 0)
                    index = (engine.State.GameCycle/actor.Cycle) & 0x3;
                else
                    index = 0;
                index += (actor.Vector.Y << 1) + 2;
                return new AnsiChar(engine.State.TransporterVChars[index], engine.Tiles[location].Color);
            }
            if (actor.Cycle > 0)
                index = (engine.State.GameCycle/actor.Cycle) & 0x3;
            else
                index = 0;
            index += (actor.Vector.X << 1) + 2;
            return new AnsiChar(engine.State.TransporterHChars[index], engine.Tiles[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            engine.PushThroughTransporter(location.Difference(vector), vector);
            vector.SetTo(0, 0);
        }
    }
}