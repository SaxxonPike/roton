using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Conditions.Impl
{
    [ContextEngine(ContextEngine.Original, "ENERGIZED")]
    [ContextEngine(ContextEngine.Super, "ENERGIZED")]
    public sealed class EnergizedCondition : ICondition
    {
        private readonly Lazy<IEngine> _engine;
        private IEngine Engine => _engine.Value;

        public EnergizedCondition(Lazy<IEngine> engine)
        {
            _engine = engine;
        }

        public bool? Execute(IOopContext context)
        {
            return Engine.World.EnergyCycles > 0;
        }
    }
}