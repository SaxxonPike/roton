using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Conditions
{
    public class AlignedCondition : ICondition
    {
        private readonly IEngine _engine;

        public AlignedCondition(IEngine engine)
        {
            _engine = engine;
        }

        public string Name => "ALLIGNED";

        public bool? Execute(IOopContext context)
        {
            return context.Actor.Location.X == _engine.Player.Location.X ||
                   context.Actor.Location.Y == _engine.Player.Location.Y;
        }
    }
}