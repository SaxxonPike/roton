using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Items
{
    [ContextEngine(ContextEngine.Zzt, "HEALTH")]
    [ContextEngine(ContextEngine.SuperZzt, "HEALTH")]
    public sealed class HealthItem : IItem
    {
        private readonly IEngine _engine;

        public HealthItem(IEngine engine)
        {
            _engine = engine;
        }

        public int Value
        {
            get => _engine.World.Health;
            set => _engine.World.Health = value;
        }
    }
}