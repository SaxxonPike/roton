using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class ClockwiseConveyorBehavior : ElementBehavior
    {
        public override string KnownName => "Clockwise Conveyor";

        public override void Act(int index)
        {
            var actor = _actorList[index];
            engine.UpdateBoard(actor.Location);
            engine.Convey(actor.Location, 1);
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            switch ((engine.State.GameCycle/engine.Elements[engine.Elements.ClockwiseId].Cycle) & 0x3)
            {
                case 0:
                    return new AnsiChar(0xB3, engine.Tiles[location].Color);
                case 1:
                    return new AnsiChar(0x2F, engine.Tiles[location].Color);
                case 2:
                    return new AnsiChar(0xC4, engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0x5C, engine.Tiles[location].Color);
            }
        }
    }
}