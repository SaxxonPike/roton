using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Conditions
{
    public class BlockedCondition : ICondition
    {
        private readonly IEngine _engine;

        public BlockedCondition(IEngine engine)
        {
            _engine = engine;
        }

        public string Name => "BLOCKED";

        public bool? Execute(IOopContext context)
        {
            var direction = _engine.Parser.GetDirection(context);
            if (direction == null)
                return null;

            return !_engine.ElementAt(context.Actor.Location.Sum(direction)).IsFloor;
        }
    }
}