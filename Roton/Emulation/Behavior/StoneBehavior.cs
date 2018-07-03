using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class StoneBehavior : ElementBehavior
    {
        public override string KnownName => "Stone of Power";

        public override void Act(IEngine engine, int index)
        {
            engine.UpdateBoard(engine.Actors[index].Location);
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            return new AnsiChar(0x41 + engine.RandomNumber(0x1A), engine.Tiles[location].Color);
        }

        public override void Interact(IEngine engine, IXyPair location, int index, IXyPair vector)
        {
            if (engine.World.Stones < 0)
            {
                engine.World.Stones = 0;
            }
            engine.World.Stones++;
            engine.Destroy(location);
            engine.UpdateStatus();
            engine.SetMessage(0xC8, engine.Alerts.StoneMessage);
        }
    }
}