using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats
{
    [ContextEngine(ContextEngine.Original, "KEYS")]
    [ContextEngine(ContextEngine.Super, "KEYS")]
    public sealed class KeysCheat : ICheat
    {
        private readonly IEngine _engine;

        public KeysCheat(IEngine engine)
        {
            _engine = engine;
        }

        public void Execute()
        {
            for (var i = 0; i < 7; i++)
                _engine.World.Keys[i] = true;
        }
    }
}