using Roton.Core;

namespace Roton.Emulation.Behavior
{
    internal class CounterclockwiseConveyorBehavior : ElementBehavior
    {
        public override string KnownName => "Counterclockwise Conveyor";

        public override void Act(IEngine engine, int index)
        {
            var actor = engine.Actors[index];
            engine.UpdateBoard(actor.Location);
            engine.Convey(actor.Location, -1);
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            switch ((engine.State.GameCycle / engine.Elements[engine.Elements.CounterId].Cycle) & 0x3)
            {
                case 3:
                    return new AnsiChar(0xB3, engine.Tiles[location].Color);
                case 2:
                    return new AnsiChar(0x2F, engine.Tiles[location].Color);
                case 1:
                    return new AnsiChar(0xC4, engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0x5C, engine.Tiles[location].Color);
            }
        }
    }
}
