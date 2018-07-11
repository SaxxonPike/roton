using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Items
{
    [ContextEngine(ContextEngine.Zzt, "AMMO")]
    [ContextEngine(ContextEngine.SuperZzt, "AMMO")]
    public sealed class AmmoItem : IItem
    {
        private readonly IEngine _engine;

        public AmmoItem(IEngine engine)
        {
            _engine = engine;
        }

        public int Value
        {
            get => _engine.World.Ammo;
            set => _engine.World.Ammo = value;
        }
    }
}