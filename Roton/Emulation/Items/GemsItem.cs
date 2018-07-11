using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Items
{
    [ContextEngine(ContextEngine.Zzt, "GEMS")]
    [ContextEngine(ContextEngine.SuperZzt, "GEMS")]
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