using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Conditions
{
    [ContextEngine(ContextEngine.Zzt, "ALLIGNED")]
    [ContextEngine(ContextEngine.SuperZzt, "ALLIGNED")]
    public sealed class AlignedCondition : ICondition
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