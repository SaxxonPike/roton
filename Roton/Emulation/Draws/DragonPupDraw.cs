using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws
{
    [ContextEngine(ContextEngine.Super, 0x3C)]
    public sealed class DragonPupDraw : IDraw
    {
        private readonly IEngine _engine;

        public DragonPupDraw(IEngine engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            switch (_engine.State.GameCycle & 0x3)
            {
                case 0:
                case 2:
                    return new AnsiChar(0x94, _engine.Tiles[location].Color);
                case 1:
                    return new AnsiChar(0xA2, _engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0x95, _engine.Tiles[location].Color);
            }
        }
    }
}