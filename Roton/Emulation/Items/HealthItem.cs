using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items
{
    [ContextEngine(ContextEngine.Original, "HEALTH")]
    [ContextEngine(ContextEngine.Super, "HEALTH")]
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