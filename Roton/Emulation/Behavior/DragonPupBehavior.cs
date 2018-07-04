using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class DragonPupBehavior : EnemyBehavior
    {
        public override string KnownName => "Dragon Pup";

        public override void Act(int index)
        {
            engine.UpdateBoard(_actorList[index].Location);
        }

        public override AnsiChar Draw(IEngine engine, IXyPair location)
        {
            switch (engine.State.GameCycle & 0x3)
            {
                case 0:
                case 2:
                    return new AnsiChar(0x94, engine.Tiles[location].Color);
                case 1:
                    return new AnsiChar(0xA2, engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0x95, engine.Tiles[location].Color);
            }
        }
    }
}