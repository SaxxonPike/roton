using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Original, "ZAP")]
    [ContextEngine(ContextEngine.Super, "ZAP")]
    public sealed class ZapCheat : ICheat
    {
        private readonly IEngine _engine;

        public ZapCheat(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute()
        {
            for (var i = 0; i < 4; i++)
            {
                _engine.Destroy(_engine.Player.Location.Sum(_engine.GetCardinalVector(i)));
            }
        }
    }
}