using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions
{
    [ContextEngine(ContextEngine.Original, "ENERGIZED")]
    [ContextEngine(ContextEngine.Super, "ENERGIZED")]
    public sealed class EnergizedCondition : ICondition
    {
        private readonly IEngine _engine;

        public EnergizedCondition(IEngine engine)
        {
            _engine = engine;
        }

        public bool? Execute(IOopContext context)
        {
            return _engine.World.EnergyCycles > 0;
        }
    }
}