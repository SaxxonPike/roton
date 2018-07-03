using Roton.Core;

namespace Roton.Emulation.Behavior
{
    public sealed class MonitorBehavior : ElementBehavior
    {
        public override string KnownName => "Monitor";

        public override void Act(IEngine engine, int index)
        {
            if (engine.State.KeyPressed != 0)
            {
                engine.State.BreakGameLoop = true;
            }
            engine.MoveActorOnRiver(index);
        }
    }
}