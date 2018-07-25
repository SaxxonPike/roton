using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions
{
    [ContextEngine(ContextEngine.Original, "NOT")]
    [ContextEngine(ContextEngine.Super, "NOT")]
    public sealed class NotCondition : ICondition
    {
        private readonly IEngine _engine;

        public NotCondition(IEngine engine)
        {
            _engine = engine;
        }

        public bool? Execute(IOopContext context)
        {
            return !_engine.Parser.GetCondition(context);
        }
    }
}