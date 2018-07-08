using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Extensions;

namespace Roton.Emulation.Cheats
{
    public class ZapCheat : ICheat
    {
        private readonly IEngine _engine;

        public ZapCheat(IEngine engine)
        {
            _engine = engine;
        }
        
        public string Name => "ZAP";
        
        public void Execute()
        {
            for (var i = 0; i < 4; i++)
            {
                _engine.Destroy(_engine.Player.Location.Sum(_engine.GetCardinalVector(i)));
            }
        }
    }
}