using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Behaviors
{
    public sealed class ClockwiseConveyorBehavior : ElementBehavior
    {
        private readonly IEngine _engine;
        public override string KnownName => KnownNames.Clockwise;

        public ClockwiseConveyorBehavior(IEngine engine)
        {
            _engine = engine;
        }

        public override void Act(int index)
        {
            var actor = _engine.Actors[index];
            _engine.UpdateBoard(actor.Location);
            _engine.Convey(actor.Location, 1);
        }

        public override AnsiChar Draw(IXyPair location)
        {
            switch ((_engine.State.GameCycle / _engine.Elements[_engine.Elements.ClockwiseId].Cycle) & 0x3)
            {
                case 0:
                    return new AnsiChar(0xB3, _engine.Tiles[location].Color);
                case 1:
                    return new AnsiChar(0x2F, _engine.Tiles[location].Color);
                case 2:
                    return new AnsiChar(0xC4, _engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0x5C, _engine.Tiles[location].Color);
            }
        }
    }
}