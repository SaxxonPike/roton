using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Behaviors
{
    public sealed class TransporterBehavior : ElementBehavior
    {
        private readonly IEngine _engine;

        public override string KnownName => KnownNames.Transporter;

        public TransporterBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            _engine.UpdateBoard(_engine.Actors[index].Location);
        }

        public override AnsiChar Draw(IXyPair location)
        {
            var actor = _engine.ActorAt(location);

            var index = actor.Cycle > 0 
                ? (_engine.State.GameCycle / actor.Cycle) & 0x3 
                : 0;
                
            if (actor.Vector.X == 0)
            {
                index += (actor.Vector.Y << 1) + 2;
                return new AnsiChar(_engine.State.TransporterVChars[index], _engine.Tiles[location].Color);
            }

            index += (actor.Vector.X << 1) + 2;
            return new AnsiChar(_engine.State.TransporterHChars[index], _engine.Tiles[location].Color);
        }

        public override void Interact(IXyPair location, int index, IXyPair vector)
        {
            _engine.PushThroughTransporter(location.Difference(vector), vector);
            vector.SetTo(0, 0);
        }
    }
}