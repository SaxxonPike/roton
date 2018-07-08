using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Roton.Emulation.Conditions
{
    public class EnergizedCondition : ICondition
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