using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Draws
{
    public class SpinningGunDraw : IDraw
    {
        private readonly IEngine _engine;

        public SpinningGunDraw(IEngine engine)
        {
            _engine = engine;
        }
        
        public AnsiChar Draw(IXyPair location)
        {
            switch (_engine.State.GameCycle & 0x7)
            {
                case 0:
                case 1:
                    return new AnsiChar(0x18, _engine.Tiles[location].Color);
                case 2:
                case 3:
                    return new AnsiChar(0x1A, _engine.Tiles[location].Color);
                case 4:
                case 5:
                    return new AnsiChar(0x19, _engine.Tiles[location].Color);
                default:
                    return new AnsiChar(0x1B, _engine.Tiles[location].Color);
            }
        }
    }
}