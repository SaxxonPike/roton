using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Items
{
    [ContextEngine(ContextEngine.Original, "GEMS")]
    [ContextEngine(ContextEngine.Super, "GEMS")]
    public sealed class GemsItem : IItem
    {
        private readonly IEngine _engine;

        public GemsItem(IEngine engine)
        {
            _engine = engine;
        }

        public int Value
        {
            get => _engine.World.Gems;
            set => _engine.World.Gems = value;
        }
    }
}