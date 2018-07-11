using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions
{
    [ContextEngine(ContextEngine.Zzt, "BLOCKED")]
    [ContextEngine(ContextEngine.SuperZzt, "BLOCKED")]
    public sealed class BlockedCondition : ICondition
    {
        private readonly IEngine _engine;

        public BlockedCondition(IEngine engine)
        {
            _engine = engine;
        }

        public bool? Execute(IOopContext context)
        {
            var direction = _engine.Parser.GetDirection(context);
            if (direction == null)
                return null;

            return !_engine.ElementAt(context.Actor.Location.Sum(direction)).IsFloor;
        }
    }
}