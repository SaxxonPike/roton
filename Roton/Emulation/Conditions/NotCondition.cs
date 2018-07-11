using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions
{
    [ContextEngine(ContextEngine.Zzt, "NOT")]
    [ContextEngine(ContextEngine.SuperZzt, "NOT")]
    public sealed class NotCondition : ICondition
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