using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items
{
    [ContextEngine(ContextEngine.Original, "TORCHES")]
    public sealed class TorchesItem : IItem
    {
        private readonly IEngine _engine;

        public TorchesItem(IEngine engine)
        {
            _engine = engine;
        }

        public int Value
        {
            get => _engine.World.Torches;
            set => _engine.World.Torches = value;
        }
    }
}