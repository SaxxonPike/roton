using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions
{
    [ContextEngine(ContextEngine.Zzt, "ENERGIZED")]
    [ContextEngine(ContextEngine.SuperZzt, "ENERGIZED")]
    public sealed class EnergizedCondition : ICondition
    {
        private readonly IEngine _engine;

        public EnergizedCondition(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "ENERGIZED";
        
        public bool? Execute(IOopContext context)
        {
            return _engine.World.EnergyCycles > 0;
        }
    }
}