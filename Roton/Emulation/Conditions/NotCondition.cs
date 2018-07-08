using Roton.Core;
using Roton.Emulation.Core;

namespace Roton.Emulation.Conditions
{
    public class NotCondition : ICondition
    {
        private readonly IEngine _engine;

        public NotCondition(IEngine engine)
        {
            _engine = engine;
        }

        public string Name => "NOT";
        
        public bool? Execute(IOopContext context)
        {
            return !_engine.Parser.GetCondition(context);
        }
    }
}