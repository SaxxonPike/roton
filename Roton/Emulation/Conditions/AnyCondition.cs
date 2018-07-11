using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions
{
    [ContextEngine(ContextEngine.Zzt, "ANY")]
    [ContextEngine(ContextEngine.SuperZzt, "ANY")]
    public sealed class AnyCondition : ICondition
    {
        private readonly IEngine _engine;

        public AnyCondition(IEngine engine)
        {
            _engine = engine;
        }

        public bool? Execute(IOopContext context)
        {
            var kind = _engine.Parser.GetKind(context);
            if (kind == null)
                return null;

            return _engine.FindTile(kind, new Location(0, 1));
        }
    }
}