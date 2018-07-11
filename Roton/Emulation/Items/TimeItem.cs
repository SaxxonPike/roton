using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Items
{
    [ContextEngine(ContextEngine.Zzt, "TIME")]
    [ContextEngine(ContextEngine.SuperZzt, "TIME")]
    public sealed class TimeItem : IItem
    {
        private readonly IEngine _engine;

        public TimeItem(IEngine engine)
        {
            _engine = engine;
        }

        public int Value
        {
            get => _engine.World.TimePassed;
            set => _engine.World.TimePassed = value;
        }
    }
}