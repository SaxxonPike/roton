using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items
{
    [ContextEngine(ContextEngine.Super, "Z")]
    public sealed class ZItem : IItem
    {
        private readonly IEngine _engine;

        public ZItem(IEngine engine)
        {
            _engine = engine;
        }

        public int Value
        {
            get => _engine.World.Stones;
            set => _engine.World.Stones = value;
        }
    }
}